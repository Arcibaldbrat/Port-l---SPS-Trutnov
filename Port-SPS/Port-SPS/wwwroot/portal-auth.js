(function () {
    const publicPages = new Set(["index.html", "login.html", "loading.html", ""]);
    const teacherPages = new Set(["classes.html", "teachers.html"]);
    const adminPages = new Set(["admin.html"]);
    const extraLinks = [
        { href: "homework.html", text: "Úkoly", roles: ["Student", "Teacher", "Admin"] },
        { href: "attendance.html", text: "Docházka", roles: ["Student", "Teacher", "Admin"] },
        { href: "calendar.html", text: "Kalendář", roles: ["Student", "Teacher", "Admin"] },
        { href: "cafeteria.html", text: "Jídelna", roles: ["Student", "Teacher", "Admin"] },
        { href: "library.html", text: "Knihovna", roles: ["Student", "Teacher", "Admin"] },
        { href: "settings.html", text: "Nastavení", roles: ["Student", "Teacher", "Admin"] },
        { href: "admin.html", text: "Admin", roles: ["Admin"] }
    ];

    function getPath() {
        return window.location.pathname.split("/").pop() || "index.html";
    }

    function getStoredUser() {
        const raw = localStorage.getItem("portalUser");
        if (!raw) {
            const legacyName = localStorage.getItem("user");
            return legacyName ? { username: legacyName, firstName: legacyName, role: "Student" } : null;
        }

        try {
            return JSON.parse(raw);
        } catch {
            localStorage.removeItem("portalUser");
            return null;
        }
    }

    function storeUser(user) {
        localStorage.setItem("portalUser", JSON.stringify(user));
        localStorage.setItem("user", user.firstName || user.username);
        localStorage.setItem("role", user.role);
    }

    function clearUser() {
        localStorage.removeItem("portalUser");
        localStorage.removeItem("user");
        localStorage.removeItem("role");
        localStorage.removeItem("rememberMe");
    }

    async function loadCurrentUser() {
        try {
            const response = await fetch("/api/auth/me", { credentials: "same-origin" });
            if (!response.ok) return null;
            const user = await response.json();
            storeUser(user);
            return user;
        } catch {
            return getStoredUser();
        }
    }

    function roleLabel(role) {
        if (role === "Teacher") return "Učitel";
        if (role === "Admin") return "Admin";
        return "Žák";
    }

    function installExtraStyles() {
        if (document.querySelector('link[href="portal-extra.css"]')) return;
        const link = document.createElement("link");
        link.rel = "stylesheet";
        link.href = "portal-extra.css";
        document.head.appendChild(link);
    }

    function installDarkMode() {
        const saved = localStorage.getItem("portalTheme");
        if (saved === "dark") document.documentElement.classList.add("dark-mode");
    }

    function addHeaderTools(user) {
        const userInfo = document.querySelector(".user-info");
        if (!userInfo || document.querySelector(".portal-tools")) return;

        const tools = document.createElement("div");
        tools.className = "portal-tools";
        tools.innerHTML = `
            <span class="role-pill">${roleLabel(user.role)}</span>
            <button class="tool-btn" type="button" id="themeToggle" title="Tmavý režim">◐</button>
        `;
        userInfo.prepend(tools);

        document.getElementById("themeToggle").addEventListener("click", () => {
            document.documentElement.classList.toggle("dark-mode");
            localStorage.setItem("portalTheme", document.documentElement.classList.contains("dark-mode") ? "dark" : "light");
        });
    }

    function enrichNavigation(user) {
        document.querySelectorAll(".nav-container").forEach((nav) => {
            extraLinks.forEach((link) => {
                if (!link.roles.includes(user.role)) return;
                if (nav.querySelector(`a[href="${link.href}"]`)) return;
                const anchor = document.createElement("a");
                anchor.href = link.href;
                anchor.className = "nav-link";
                anchor.textContent = link.text;
                if (getPath() === link.href) anchor.classList.add("active");
                nav.appendChild(anchor);
            });
        });

        if (user.role === "Student") {
            document.querySelectorAll('a[href="classes.html"], a[href="teachers.html"]').forEach((link) => {
                link.style.display = "none";
            });
        }
    }

    function applySearch() {
        const container = document.querySelector(".container");
        if (!container || document.querySelector(".portal-search")) return;

        const search = document.createElement("div");
        search.className = "portal-search";
        search.innerHTML = '<input type="search" placeholder="Hledat na této stránce..." aria-label="Hledat">';
        container.prepend(search);

        const input = search.querySelector("input");
        input.addEventListener("input", () => {
            const value = input.value.trim().toLowerCase();
            document.querySelectorAll(".card, .subject-card, .announcement, .teacher-card, .class-card, .portal-card, .materials-list").forEach((item) => {
                item.style.display = !value || item.innerText.toLowerCase().includes(value) ? "" : "none";
            });
        });
    }

    function applyUserToPage(user) {
        const name = user.firstName && user.lastName ? `${user.firstName} ${user.lastName}` : user.username;
        document.querySelectorAll("#userGreeting, .user-greeting").forEach((element) => {
            element.textContent = `Dobrý den, ${name}!`;
        });
        document.querySelectorAll("[data-role-label]").forEach((element) => {
            element.textContent = roleLabel(user.role);
        });

        installExtraStyles();
        installDarkMode();
        addHeaderTools(user);
        enrichNavigation(user);
        applySearch();
    }

    async function ensureAuthenticated() {
        const path = getPath();
        installExtraStyles();
        installDarkMode();

        if (publicPages.has(path)) return;

        const user = await loadCurrentUser();
        if (!user) {
            clearUser();
            window.location.href = "login.html";
            return;
        }

        if ((teacherPages.has(path) && user.role === "Student") || (adminPages.has(path) && user.role !== "Admin")) {
            window.location.href = "dashboard.html";
            return;
        }

        applyUserToPage(user);
    }

    window.portalAuth = {
        getStoredUser,
        storeUser,
        clearUser,
        loadCurrentUser,
        applyUserToPage,
        async logout() {
            try {
                await fetch("/api/auth/logout", { method: "POST", credentials: "same-origin" });
            } finally {
                clearUser();
                window.location.href = "login.html";
            }
        }
    };

    window.handleLogout = window.portalAuth.logout;
    document.addEventListener("DOMContentLoaded", ensureAuthenticated);
})();

document.addEventListener("DOMContentLoaded", () => {
    const dashboard = document.querySelector("#dashboard .dashboard-grid");
    if (!dashboard || document.querySelector(".portal-dashboard-extra")) return;

    const wrap = document.createElement("div");
    wrap.className = "card portal-dashboard-extra";
    wrap.innerHTML = `
        <h3>Rychlý přístup</h3>
        <div class="portal-grid">
            <a class="portal-card" href="homework.html"><h3>Úkoly</h3><p>Odevzdání, termíny a stav splnění.</p></a>
            <a class="portal-card" href="attendance.html"><h3>Docházka</h3><p>Absence, omluvenky a pozdní příchody.</p></a>
            <a class="portal-card" href="calendar.html"><h3>Kalendář</h3><p>Akce školy, testy a třídní schůzky.</p></a>
            <a class="portal-card" href="cafeteria.html"><h3>Jídelna</h3><p>Menu, alergeny a objednávky.</p></a>
            <a class="portal-card" href="library.html"><h3>Knihovna</h3><p>Katalog knih a rezervace.</p></a>
            <a class="portal-card" href="settings.html"><h3>Nastavení</h3><p>Notifikace, vzhled a zabezpečení.</p></a>
        </div>
    `;
    dashboard.prepend(wrap);
});

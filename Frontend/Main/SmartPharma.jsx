import { useState } from "react";
import { NAV_ITEMS, REPORTS_ITEMS } from "./data.js";
import DashboardPage     from "./DashboardPage.jsx";
import InventoryPage     from "./InventoryPage.jsx";
import PrescriptionsPage from "./PrescriptionsPage.jsx";
import CatalogPage       from "./CatalogPage.jsx";

const GLOBAL_STYLES = `
  @import url('https://fonts.googleapis.com/css2?family=DM+Sans:wght@400;500;600;700&display=swap');
  * { box-sizing: border-box; margin: 0; padding: 0; }
  ::-webkit-scrollbar { width: 6px; }
  ::-webkit-scrollbar-track { background: #1e2738; }
  ::-webkit-scrollbar-thumb { background: #3b4a63; border-radius: 3px; }
  .nav-item { display:flex; align-items:center; gap:10px; padding:9px 16px; border-radius:8px; cursor:pointer; color:#94a3b8; font-size:14px; font-weight:500; transition:all .15s; }
  .nav-item:hover { background:#ffffff12; color:#fff; }
  .nav-item.active { background:#2563eb; color:#fff; }
  .btn { border:none; cursor:pointer; font-family:inherit; font-weight:600; border-radius:8px; transition:all .15s; }
  .btn:hover { opacity:.88; }
  .pill { display:inline-flex; align-items:center; padding:2px 10px; border-radius:99px; font-size:12px; font-weight:600; }
  .filter-btn { border:1.5px solid #e2e8f0; background:#fff; cursor:pointer; padding:6px 16px; border-radius:8px; font-size:13px; font-weight:600; color:#475569; transition:all .15s; }
  .filter-btn:hover { border-color:#2563eb; color:#2563eb; }
  .filter-btn.active { background:#1e40af; border-color:#1e40af; color:#fff; }
  .card { background:#fff; border-radius:14px; box-shadow:0 1px 4px #0001; }
  .rx-item { display:flex; align-items:center; gap:12px; padding:14px; border-radius:10px; cursor:pointer; transition:background .12s; border:1.5px solid transparent; }
  .rx-item:hover { background:#f8fafc; }
  .rx-item.active { background:#eff6ff; border-color:#bfdbfe; }
  input[type=range] { accent-color:#2563eb; }
`;

const RX_NAV = [
  { id: "prescriptions", label: "Workstation",   icon: "🖥" },
  { id: "prescriptions", label: "Prescriptions", icon: "💊", badge: true },
  { id: "prescriptions", label: "Dispense Queue",icon: "📋" },
  { id: "prescriptions", label: "Stock Check",   icon: "📦" },
  { id: "prescriptions", label: "Patients",      icon: "" },
  { id: "prescriptions", label: "Consult Chat",  icon: "💬" },
];

export default function SmartPharma() {
  const [page, setPage] = useState("dashboard");

  const isRx      = page === "prescriptions";
  const isCatalog = page === "catalog";

  /* ── Catalog has its own full-screen layout ── */
  if (isCatalog) {
    return (
      <div style={{ fontFamily: "'DM Sans','Segoe UI',sans-serif" }}>
        <style>{GLOBAL_STYLES}</style>
        <CatalogPage setPage={setPage} />
      </div>
    );
  }

  /* ── Admin / Pharmacist shell ── */
  return (
    <div style={{ fontFamily: "'DM Sans','Segoe UI',sans-serif", background: "#f1f5f9", minHeight: "100vh" }}>
      <style>{GLOBAL_STYLES}</style>

      <div style={{ display: "flex", height: "100vh", overflow: "hidden" }}>

        {/* ── Sidebar ── */}
        <aside style={{ width: 210, background: "#0f172a", display: "flex", flexDirection: "column", padding: "20px 12px", flexShrink: 0 }}>

          {/* Logo */}
          <div style={{ padding: "4px 8px 20px", borderBottom: "1px solid #ffffff15" }}>
            <div style={{ fontSize: 18, fontWeight: 800, color: "#fff" }}>
              Smart<span style={{ color: "#3b82f6" }}>Pharma</span>
            </div>
            <div style={{ fontSize: 11, color: "#64748b", marginTop: 2 }}>
              {isRx ? "Pharmacist Workstation" : "Admin Console"}
            </div>
          </div>

          {/* Main nav */}
          <div style={{ marginTop: 20 }}>
            <div style={{ fontSize: 10, color: "#475569", fontWeight: 700, letterSpacing: "0.1em", padding: "0 8px", marginBottom: 6 }}>MAIN</div>
            {(isRx ? RX_NAV : NAV_ITEMS).map((item, i) => (
              <div
                key={i}
                className={`nav-item ${page === item.id ? "active" : ""}`}
                onClick={() => setPage(item.id)}
              >
                <span>{item.icon}</span>
                <span>{item.label}</span>
                {item.badge && (
                  <span style={{ marginLeft: "auto", background: "#ef4444", color: "#fff", fontSize: 10, fontWeight: 700, borderRadius: 99, padding: "1px 6px" }}>8</span>
                )}
              </div>
            ))}
          </div>

          {/* Reports section (admin only) */}
          {!isRx && (
            <div style={{ marginTop: 24 }}>
              <div style={{ fontSize: 10, color: "#475569", fontWeight: 700, letterSpacing: "0.1em", padding: "0 8px", marginBottom: 6 }}>REPORTS</div>
              {REPORTS_ITEMS.map(item => (
                <div key={item.id} className={`nav-item ${page === item.id ? "active" : ""}`} onClick={() => setPage(item.id)}>
                  <span>{item.icon}</span><span>{item.label}</span>
                </div>
              ))}
            </div>
          )}

          {/* Catalog shortcut */}
          <div style={{ marginTop: 12 }}>
            <div
              className="nav-item"
              onClick={() => setPage("catalog")}
              style={{ border: "1px dashed #334155", marginTop: 4 }}
            >
              <span>🛒</span><span>Customer Catalog</span>
            </div>
          </div>

          {/* User footer */}
          <div style={{ marginTop: "auto", borderTop: "1px solid #ffffff15", paddingTop: 16 }}>
            <div style={{ fontSize: 10, color: "#475569", fontWeight: 700, letterSpacing: "0.1em", padding: "0 8px 8px" }}>
              <span style={{ marginRight: 6 }}>⚙️</span>Settings
            </div>
            <div style={{ display: "flex", alignItems: "center", gap: 10, padding: "8px" }}>
              <div style={{ width: 32, height: 32, borderRadius: "50%", background: "#2563eb", display: "flex", alignItems: "center", justifyContent: "center", color: "#fff", fontSize: 12, fontWeight: 700, flexShrink: 0 }}>
                {isRx ? "DA" : "AM"}
              </div>
              <div>
                <div style={{ fontSize: 13, color: "#e2e8f0", fontWeight: 600 }}>{isRx ? "Dana Aldea" : "Alex Marin"}</div>
                <div style={{ fontSize: 11, color: "#64748b" }}>{isRx ? "Pharmacist" : "Admin"}</div>
              </div>
            </div>
          </div>
        </aside>

        {/* ── Main content ── */}
        <main style={{ flex: 1, overflow: "auto", display: "flex", flexDirection: "column" }}>
          {page === "dashboard"     && <DashboardPage     setPage={setPage} />}
          {page === "inventory"     && <InventoryPage />}
          {page === "prescriptions" && <PrescriptionsPage />}

          {/* Placeholder for unbuilt pages */}
          {["orders", "users", "analytics", "finance"].includes(page) && (
            <div style={{ flex: 1, display: "flex", alignItems: "center", justifyContent: "center", color: "#94a3b8" }}>
              <div style={{ textAlign: "center" }}>
                <div style={{ fontSize: 48, marginBottom: 12 }}>🚧</div>
                <div style={{ fontWeight: 600, fontSize: 18 }}>{page.charAt(0).toUpperCase() + page.slice(1)} page</div>
                <div style={{ fontSize: 14, marginTop: 6 }}>Coming soon</div>
              </div>
            </div>
          )}
        </main>
      </div>
    </div>
  );
}

import { useState } from "react";
import { CATALOG_DATA } from "./data.js";

export default function CatalogPage({ setPage }) {
  const [catFilter, setCatFilter] = useState("All");
  const [catSearch, setCatSearch] = useState("");
  const [cart, setCart] = useState(2);

  return (
    <div style={{ background: "#f8fafc", minHeight: "100vh" }}>
      {/* Top nav */}
      <nav style={{ background: "#fff", borderBottom: "1.5px solid #f1f5f9", padding: "14px 32px", display: "flex", alignItems: "center", gap: 28 }}>
        <div style={{ fontSize: 18, fontWeight: 800, color: "#0f172a", cursor: "pointer" }} onClick={() => setPage("dashboard")}>
          Smart<span style={{ color: "#2563eb" }}>Pharma</span>
        </div>
        {["Home", "Catalog", "Prescriptions", "My Orders"].map(n => (
          <span key={n} style={{ fontSize: 14, color: n === "Catalog" ? "#2563eb" : "#475569", fontWeight: n === "Catalog" ? 700 : 500, cursor: "pointer", borderBottom: n === "Catalog" ? "2px solid #2563eb" : "none", paddingBottom: 2 }}>{n}</span>
        ))}
        <div style={{ flex: 1, display: "flex", alignItems: "center", gap: 8, background: "#f8fafc", borderRadius: 8, padding: "8px 12px", border: "1.5px solid #e2e8f0" }}>
          <span style={{ color: "#94a3b8" }}>🔍</span>
          <input value={catSearch} onChange={e => setCatSearch(e.target.value)} placeholder="Search products..." style={{ border: "none", background: "transparent", outline: "none", fontSize: 13, color: "#475569", flex: 1 }} />
        </div>
        <button className="btn" style={{ background: "#0f172a", color: "#fff", padding: "8px 18px", fontSize: 13, display: "flex", alignItems: "center", gap: 6 }} onClick={() => setPage("dashboard")}>
          🛒 Cart ({cart})
        </button>
        <div style={{ width: 32, height: 32, borderRadius: "50%", background: "#2563eb", display: "flex", alignItems: "center", justifyContent: "center", color: "#fff", fontSize: 12, fontWeight: 700 }}>IP</div>
      </nav>

      <div style={{ display: "flex", padding: "24px 32px", gap: 24 }}>
        {/* Category sidebar */}
        <aside style={{ width: 180, flexShrink: 0 }}>
          <div style={{ marginBottom: 18 }}>
            <div style={{ fontSize: 10, fontWeight: 700, color: "#94a3b8", letterSpacing: "0.1em", marginBottom: 10 }}>CATEGORIES</div>
            {["All Products", "Prescription", "Supplements", "Cardiology", "Antibiotics", "Dermatology", "Pediatric", "Mental Health", "Orthopedic"].map(c => (
              <div key={c} style={{ display: "flex", alignItems: "center", gap: 8, padding: "7px 0", cursor: "pointer", color: c === "All Products" ? "#2563eb" : "#475569", fontWeight: c === "All Products" ? 700 : 500, fontSize: 13, borderLeft: c === "All Products" ? "3px solid #2563eb" : "3px solid transparent", paddingLeft: 8 }}>
                {c}
              </div>
            ))}
          </div>
          <div style={{ marginBottom: 18 }}>
            <div style={{ fontSize: 10, fontWeight: 700, color: "#94a3b8", letterSpacing: "0.1em", marginBottom: 10 }}>AVAILABILITY</div>
            {["In stock only", "Rx required", "On sale"].map((a, i) => (
              <div key={a} style={{ display: "flex", alignItems: "center", gap: 8, padding: "5px 0", fontSize: 13, color: "#475569", cursor: "pointer" }}>
                <input type="checkbox" defaultChecked={i === 0} style={{ accentColor: "#2563eb" }} /> {a}
              </div>
            ))}
          </div>
          <div>
            <div style={{ fontSize: 10, fontWeight: 700, color: "#94a3b8", letterSpacing: "0.1em", marginBottom: 10 }}>PRICE (RON)</div>
            <input type="range" min={0} max={500} defaultValue={500} style={{ width: "100%" }} />
            <div style={{ display: "flex", justifyContent: "space-between", fontSize: 11, color: "#94a3b8" }}><span>0</span><span>500+</span></div>
          </div>
        </aside>

        {/* Product grid */}
        <div style={{ flex: 1 }}>
          <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginBottom: 14 }}>
            <div style={{ display: "flex", gap: 8 }}>
              {["All", "OTC", "Prescription", "On sale", "New arrivals"].map(f => (
                <button key={f} className={`btn filter-btn ${catFilter === f ? "active" : ""}`} onClick={() => setCatFilter(f)} style={{ fontSize: 12 }}>{f}</button>
              ))}
            </div>
            <div style={{ display: "flex", alignItems: "center", gap: 10 }}>
              <span style={{ fontSize: 13, color: "#64748b" }}>384 products</span>
              <select style={{ border: "1.5px solid #e2e8f0", borderRadius: 8, padding: "6px 12px", fontSize: 13, color: "#475569", outline: "none" }}>
                <option>Sort: Relevance</option>
              </select>
            </div>
          </div>

          <div style={{ display: "grid", gridTemplateColumns: "repeat(4,1fr)", gap: 14 }}>
            {CATALOG_DATA.map((p, i) => (
              <div key={i} className="card" style={{ padding: 16, display: "flex", flexDirection: "column" }}>
                <div style={{ background: "#f8fafc", borderRadius: 10, height: 90, display: "flex", alignItems: "center", justifyContent: "center", fontSize: 38, marginBottom: 12 }}>{p.emoji}</div>
                <div style={{ fontWeight: 700, fontSize: 13, color: "#0f172a", marginBottom: 2 }}>{p.name}</div>
                <div style={{ fontSize: 11, color: "#94a3b8", marginBottom: 8 }}>{p.sub}</div>
                <span className="pill" style={{ background: p.badgeColor + "18", color: p.badgeColor, fontSize: 10, marginBottom: 10, alignSelf: "flex-start" }}>{p.badge}</span>
                <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginTop: "auto" }}>
                  <div>
                    <div style={{ fontWeight: 800, fontSize: 16, color: "#0f172a" }}>{p.price} RON</div>
                    <div style={{ fontSize: 11, color: p.inStock ? "#10b981" : "#f59e0b", fontWeight: 600 }}>{p.inStock ? "In stock" : "Low stock"}</div>
                  </div>
                  <button
                    className="btn"
                    onClick={() => p.inStock && setCart(c => c + 1)}
                    style={{ background: p.inStock ? "#2563eb" : "#e2e8f0", color: p.inStock ? "#fff" : "#94a3b8", padding: "7px 16px", fontSize: 12 }}
                  >
                    {p.inStock ? "Add" : "Notify"}
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}

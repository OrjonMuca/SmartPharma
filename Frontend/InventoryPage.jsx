import { useState } from "react";
import { INVENTORY_DATA, statusColor, levelColor } from "./data.js";

export default function InventoryPage() {
  const [invFilter, setInvFilter] = useState("All");

  const filteredInv = INVENTORY_DATA.filter(p => {
    if (invFilter === "Low Stock") return p.status === "Low" || p.status === "Critical";
    if (invFilter === "Out of Stock") return p.status === "Out of Stock";
    if (invFilter === "Rx Only") return p.category === "Antibiotic" || p.category === "Cardiology";
    return true;
  });

  return (
    <div style={{ padding: 28 }}>
      {/* Header */}
      <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginBottom: 6 }}>
        <div>
          <h1 style={{ fontSize: 22, fontWeight: 700, color: "#0f172a" }}>Inventory Management</h1>
          <p style={{ color: "#94a3b8", fontSize: 13, marginTop: 2 }}>2,148 products across 12 categories</p>
        </div>
        <div style={{ display: "flex", gap: 10 }}>
          <button className="btn filter-btn">Export CSV</button>
          <button className="btn filter-btn" style={{ display: "flex", alignItems: "center", gap: 6 }}>🏷 Import</button>
          <button className="btn" style={{ background: "#0f172a", color: "#fff", padding: "8px 16px", fontSize: 13 }}>+ Add Product</button>
        </div>
      </div>

      {/* KPI cards */}
      <div style={{ display: "grid", gridTemplateColumns: "repeat(4,1fr)", gap: 12, margin: "18px 0" }}>
        {[
          { label: "Total SKUs",    value: "2,148", sub: "↑ 14 added this week",      sColor: "#475569" },
          { label: "In Stock",      value: "1,986", sub: "92.5% availability",         sColor: "#10b981" },
          { label: "Low Stock",     value: "7",     sub: "Below reorder threshold",    sColor: "#f59e0b" },
          { label: "Out of Stock",  value: "3",     sub: "Reorder pending",            sColor: "#ef4444" },
        ].map((k, i) => (
          <div key={i} className="card" style={{ padding: "16px 18px" }}>
            <div style={{ fontSize: 11, color: "#94a3b8", fontWeight: 700, letterSpacing: "0.07em", marginBottom: 6 }}>{k.label.toUpperCase()}</div>
            <div style={{ fontSize: 28, fontWeight: 800, color: "#0f172a" }}>{k.value}</div>
            <div style={{ fontSize: 12, color: k.sColor, marginTop: 3 }}>{k.sub}</div>
          </div>
        ))}
      </div>

      {/* Filters */}
      <div className="card" style={{ padding: "14px 16px", marginBottom: 12 }}>
        <div style={{ display: "flex", alignItems: "center", gap: 12 }}>
          <div style={{ flex: 1, display: "flex", alignItems: "center", gap: 8, background: "#f8fafc", borderRadius: 8, padding: "8px 12px", border: "1.5px solid #e2e8f0" }}>
            <span style={{ color: "#94a3b8" }}>🔍</span>
            <input placeholder="Search by name, SKU, category..." style={{ border: "none", background: "transparent", outline: "none", fontSize: 13, color: "#475569", flex: 1 }} />
          </div>
          <span style={{ color: "#64748b", fontSize: 13, fontWeight: 600 }}>Filter:</span>
          {["All", "Low Stock", "Out of Stock", "Rx Only"].map(f => (
            <button key={f} className={`btn filter-btn ${invFilter === f ? "active" : ""}`} onClick={() => setInvFilter(f)}>{f}</button>
          ))}
        </div>
      </div>

      {/* Table */}
      <div className="card" style={{ overflow: "hidden" }}>
        <table style={{ width: "100%", borderCollapse: "collapse" }}>
          <thead style={{ background: "#f8fafc" }}>
            <tr>
              {["PRODUCT", "CATEGORY", "SKU", "STOCK", "LEVEL", "REORDER AT", "UNIT PRICE", "STATUS", "ACTION"].map(h => (
                <th key={h} style={{ textAlign: "left", padding: "10px 14px", fontSize: 11, color: "#94a3b8", fontWeight: 700, letterSpacing: "0.07em" }}>{h}</th>
              ))}
            </tr>
          </thead>
          <tbody>
            {filteredInv.map((p, i) => (
              <tr key={i} style={{ borderTop: "1px solid #f1f5f9" }}>
                <td style={{ padding: "13px 14px", fontWeight: 600, fontSize: 13, color: "#0f172a" }}>{p.name}</td>
                <td style={{ padding: "13px 14px" }}>
                  <span style={{ display: "flex", alignItems: "center", gap: 6, fontSize: 13, color: "#475569" }}>
                    <span style={{ width: 8, height: 8, borderRadius: "50%", background: "#2563eb", display: "inline-block" }} />
                    {p.category}
                  </span>
                </td>
                <td style={{ padding: "13px 14px", fontSize: 12, color: "#64748b", fontFamily: "monospace" }}>{p.sku}</td>
                <td style={{ padding: "13px 14px", fontWeight: 700, fontSize: 13, color: "#0f172a" }}>{p.stock}</td>
                <td style={{ padding: "13px 14px", width: 120 }}>
                  <div style={{ background: "#e2e8f0", borderRadius: 99, height: 6, overflow: "hidden" }}>
                    <div style={{ height: "100%", borderRadius: 99, width: `${Math.min(p.level * 100, 100)}%`, background: levelColor(p.status) }} />
                  </div>
                </td>
                <td style={{ padding: "13px 14px", fontSize: 13, color: "#64748b" }}>{p.reorderAt} units</td>
                <td style={{ padding: "13px 14px", fontWeight: 600, fontSize: 13, color: "#0f172a" }}>{p.unitPrice}</td>
                <td style={{ padding: "13px 14px" }}>
                  <span className="pill" style={{ background: statusColor(p.status) + "18", color: statusColor(p.status), fontSize: 11 }}>{p.status}</span>
                </td>
                <td style={{ padding: "13px 14px" }}>
                  <span style={{ color: "#2563eb", fontSize: 13, cursor: "pointer", fontWeight: 600 }}>
                    {p.status === "In Stock" ? "Edit" : "Reorder"}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

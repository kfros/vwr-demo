import { Link } from "@tanstack/react-router";
import { api } from "../lib/api";
import React from "react";

export default function Home() {
  const [loading, setLoading] = React.useState(false);
  const [lastTx, setLastTx] = React.useState<string | null>(null);
  const [error, setError] = React.useState<string | null>(null);

  const publishDemo = async () => {
    setError(null);
    setLastTx(null);
    setLoading(true);
    try {
      const base = api.defaults.baseURL!.replace(/\/+$/, "");
      const res = await fetch(
        `${base}/api/Receipts/publish?take=8&metadata=ipfs://demo`,
        {
          method: "POST",
          mode: "cors",
        }
      );
      const data = await res.json();
      if (!res.ok) throw new Error(data?.error ?? res.statusText);
      setLastTx(data?.txHash ?? "(tx hash not returned)");
    } catch (e: any) {
      setError(e.message ?? String(e));
    } finally {
      setLoading(false);
    }
  };

 
  return (
    <div className="space-y-8">
      {/* HERO */}
      <section className="text-center space-y-4">
        <h1 className="text-4xl md:text-5xl font-semibold tracking-tight">
          Webhook → Merkle → On-chain
        </h1>
        <p className="text-slate-600 max-w-xl mx-auto">
          Pipeline demo: get webhooks, log events, build Merkle tree, publish
          on-chain receipt.
        </p>

        <div className="flex justify-center gap-3 pt-2">
          <Link to="/events" className="btn btn-ghost">
            Watch events
          </Link>
          <button
            className="btn btn-primary"
            onClick={publishDemo}
            disabled={loading}
            title="POST /api/Receipts/publish?take=8"
          >
            {loading ? "Publishing..." : "Publish demo-receipt"}
          </button>
          <Link to="/receipts" className="btn btn-ghost">
            View receipts
          </Link>
        </div>

        {(lastTx || error) && (
          <div className="max-w-xl mx-auto">
            <div
              className={`card mt-4 ${error ? "border-red-200" : "border-emerald-200"}`}
            >
              {error ? (
                <p className="text-sm text-red-600">Publish error: {error}</p>
              ) : (
                <p className="text-sm">
                  Publish succeed. Tx:{" "}
                  <span className="font-mono break-all">{lastTx}</span>
                </p>
              )}
            </div>
          </div>
        )}
      </section>

      {/* FEATURES */}
      <section className="grid sm:grid-cols-3 gap-4">
        <div className="card">
          <h3 className="font-medium mb-1">Webhook intake</h3>
          <p className="text-sm text-slate-600">
            Receive POST-payload, log and save to Postgres.
          </p>
        </div>
        <div className="card">
          <h3 className="font-medium mb-1">Merkle aggregation</h3>
          <p className="text-sm text-slate-600">
            Build leaves keccak256(payload), form Merkle root.
          </p>
        </div>
        <div className="card">
          <h3 className="font-medium mb-1">On-chain receipt</h3>
          <p className="text-sm text-slate-600">
            Publish root and metadata URI to AttestationRegistry contract.
          </p>
        </div>
      </section>
    </div>
  );
}

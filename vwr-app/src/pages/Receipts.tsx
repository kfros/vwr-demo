import React from "react";
import { api } from "../lib/api";
import { useQuery } from "@tanstack/react-query";

type ReceiptDTO = {
  id: string;
  eventId: string;
  merkleRoot: string;
    txHash: string | null;
    metaDataUri: string | null;
    publishedAt: string | null;
};

const BASE = (import.meta.env.VITE_API_URL ?? 'http://localhost:5248').replace(/\/+$/, '');

export function Receipts() {
  const { data, error, isLoading } = useQuery<ReceiptDTO[], Error>({
    queryKey: ['receipt'],
    queryFn: async () => {
      const res = await fetch(`${BASE}/api/Receipts`, { mode: 'cors' });
      if (!res.ok) {
        throw new Error(`Error fetching latest receipt: ${res.statusText}`);
      }
      return res.json();
    },
    refetchInterval: 10000, // 🔁 auto-refresh each 10 sec
  });
    if (isLoading) return <section className="p-2">Loading…</section>;
    if (error) return <section className="p-2 text-red-600">Error: {error.message}</section>;

    return (
        <section className="p-2">
            <h2 className="text-lg font-medium mb-2">Latest Receipts</h2>
            {data && data.length > 0 ? (
                <ul>
                    {data.map((receipt) => (
                        <li key={receipt.id} className="mb-2">
                            <strong>Receipt ID:</strong> {receipt.id}<br />
                            <strong>Event ID:</strong> {receipt.eventId}<br />
                            <strong>Merkle Root:</strong> {receipt.merkleRoot}<br />
                            <strong>Transaction Hash:</strong> {receipt.txHash ?? 'N/A'}<br />
                            <strong>Metadata URI:</strong> {receipt.metaDataUri ?? 'N/A'}<br />
                            <strong>Published At:</strong> {receipt.publishedAt ?? 'N/A'}
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No receipts available.</p>
            )}
        </section>
    );
}
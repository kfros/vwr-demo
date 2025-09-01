# VWR – Webhook → Merkle → On-chain Demo 🚀

**Webhook → Merkle → On-chain (VWR)** is a full-stack demo showing how traditional Web2 events (webhooks) can be verified, aggregated, and published into a smart contract on Ethereum.

- ✅ **Web2**: receive arbitrary webhooks via REST API (.NET 8 + PostgreSQL)  
- ✅ **Merkle**: store events, compute Merkle roots, and prepare receipts  
- ✅ **Web3**: publish receipts into a deployed Solidity contract via Hardhat + Ethers  
- ✅ **UI**: React + Tailwind frontend to view events, simulate webhooks, and interact with the blockchain  

---

## ✨ Features
- 🌐 **Webhook ingestion** (`/api/webhooks/in/{source}`) – send events from GitHub, Slack, Jira, or demo payloads  
- 📦 **Event storage** – PostgreSQL + EF Core migrations  
- 🌳 **Merkle proof** – events can be bundled into Merkle roots for receipts  
- ⛓ **On-chain publishing** – AttestationRegistry contract deployed locally via Hardhat  
- 🖥 **Frontend** – React + Vite + TanStack Router + Wagmi wallet connect  
- 🔥 **Out-of-the-box** – Docker Compose spins up the backend and database  

---

## 🛠 Stack
- **Backend**: ASP.NET Core 8, Entity Framework Core, Nethereum  
- **Frontend**: React 18 (Vite, TypeScript, Tailwind, Wagmi, TanStack Router)  
- **Blockchain**: Solidity, Hardhat, Ethers.js  
- **Database**: PostgreSQL 16  
- **DevOps**: Docker Compose, Node.js 22+, NVM, PowerShell/WSL2  

---

## 🚀 Quickstart

Clone the repo:
```bash
git clone https://github.com/YOURNAME/vwr-demo.git
cd vwr-demo

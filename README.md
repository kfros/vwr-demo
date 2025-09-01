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
git clone https://github.com/kfros/vwr-demo.git
cd vwr-demo
```
**## 🧪 Run locally with Docker**

> Requirements: Docker Desktop (WSL2 on Windows recommended)

1) Copy `.env.example` to `.env` (or create `.env` as below) and **leave `EVM__CONTRACTADDRESS` empty** for now:
```bash
POSTGRES_DB=vwr
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
ConnectionStrings__pg=Host=db;Port=5432;Database=vwr;Username=postgres;Password=postgres
CORS__AllowedOrigins=http://localhost:5173,http://localhost:8080
EVM__RPCURL=http://hardhat:8545
EVM__PRIVATEKEY=0x59c6995e998f97a5a0044976f6e79df4f5b1b6f4ff88e84aa2c9a2e2937c5fdc
# EVM__CONTRACTADDRESS= (fill after deploy)
```
2)Start the stack:
```bash
docker compose up -d


Get the deployed contract address from deploy logs:

docker logs vwr-deploy | grep "deployed to"
# ✅ AttestationRegistry deployed to: 0xABCDEF...


Put it into .env:

EVM__CONTRACTADDRESS=0xABCDEF...


Recreate the API container so it picks up the address:

docker compose up -d --build api


Open:

API (Swagger): http://localhost:7020/swagger

Web UI: http://localhost:8080

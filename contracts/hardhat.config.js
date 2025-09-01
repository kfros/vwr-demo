import hardhatEthers from "@nomicfoundation/hardhat-ethers";

/** @type {import('hardhat/config').HardhatUserConfig} */
const config = {
  plugins: [hardhatEthers],
  solidity: "0.8.28",
  networks: {
    hardhat:  { type: "edr-simulated", chainId: 31337 },
    localhost:{ type: "http", url: "http://127.0.0.1:8545" }
  }
};

export default config;

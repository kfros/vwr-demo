import { network } from "hardhat";

async function main() {
  const { ethers } = await network.connect();

  const [deployer] = await ethers.getSigners();
  console.log("Deployer:", deployer.address);

  const F = await ethers.getContractFactory("AttestationRegistry");
  const c = await F.deploy();
  await c.waitForDeployment();

  console.log("AttestationRegistry deployed to:", await c.getAddress());
}

main().catch((e) => { console.error(e); process.exitCode = 1; });
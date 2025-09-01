pragma solidity ^0.8.20;

contract AttestationRegistry {
    event ReceiptPublished(
        bytes32 indexed merkleRoot,
        address indexed publisher,
        string metadataURI
    );
    mapping(bytes32 => uint256) public publishedAt;
    function publish(bytes32 merkleRoot, string calldata metadataURI) external {
        require(publishedAt[merkleRoot] == 0, "already");
        publishedAt[merkleRoot] = block.timestamp;
        emit ReceiptPublished(merkleRoot, msg.sender, metadataURI);
    }
    function isPublished(bytes32 merkleRoot) external view returns (bool) {
        return publishedAt[merkleRoot] != 0;
    }
}

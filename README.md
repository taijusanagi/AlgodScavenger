# AlgodScavenger

Welcome to AlgodScavenger! In this adventure, you dive into the world of Algorand blockchain, searching for the elusive Algod Stone hidden in the mined blocks. Each block you explore brings you closer to this rare gem. Get ready for a journey full of discovery and excitement in the digital depths of Algorand!

## Reference

### Algoland Unity SDK

The official documentation is here.

https://careboo.github.io/unity-algorand-sdk/5.0/manual/getting_started/your_first_transaction.html

#### Investigation

Please check ArgorandSDKInvestigationScene in Unity for the detail.

In the scene, the following functionalitis were tested.

- Create account
- Get balance
- Send transaction
- Interact with smart contract

https://github.com/taijusanagi/AlgodScavenger/blob/main/Assets/Scripts/AlgodManager.cs

#### Technical Challenge

- Get balance with client in official doc is not working, so prepare code to interact with API directory.

## Asset from Unity Asset Store

For this hackathon, we focused on integrating Algorand blockchain into game, so game part is almost from this tutorial.

https://learn.unity.com/project/2d-roguelike-tutorial

Based on the tutorial, we added the followings

- Original story for Algod Stone and Scavenger
- Create Algorand local wallet
- Send Algorand tx for game start
- Transfer Algorand ASA for Algod Stone picking
- Required UI for the Algorand integration

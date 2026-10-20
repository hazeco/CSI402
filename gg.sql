-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: csi402db
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `cartitems`
--

DROP TABLE IF EXISTS `cartitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cartitems` (
  `CartItemId` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `ProductId` int NOT NULL,
  `Quantity` int NOT NULL DEFAULT '1',
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`CartItemId`),
  UNIQUE KEY `uq_cart_user_product` (`UserId`,`ProductId`),
  KEY `fk_cart_product` (`ProductId`),
  CONSTRAINT `fk_cart_product` FOREIGN KEY (`ProductId`) REFERENCES `products` (`ProductId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_cart_user` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `chk_cart_qty` CHECK ((`Quantity` > 0))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cartitems`
--

LOCK TABLES `cartitems` WRITE;
/*!40000 ALTER TABLE `cartitems` DISABLE KEYS */;
/*!40000 ALTER TABLE `cartitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `CategoryId` int NOT NULL AUTO_INCREMENT,
  `CategoryName` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Slug` varchar(100) NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`CategoryId`),
  UNIQUE KEY `uq_categories_slug` (`Slug`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'สเกลฟิกเกอร์ (Scale Figures)','scale-figures',1);
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `financialledger`
--

DROP TABLE IF EXISTS `financialledger`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `financialledger` (
  `EntryId` int NOT NULL AUTO_INCREMENT,
  `EntryType` varchar(20) NOT NULL DEFAULT 'expense',
  `CategoryName` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Description` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Amount` decimal(12,2) NOT NULL,
  `EntryDate` date NOT NULL,
  `RecordedByUserId` int NOT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`EntryId`),
  KEY `idx_ledger_date_type` (`EntryDate`,`EntryType`),
  KEY `fk_ledger_user` (`RecordedByUserId`),
  CONSTRAINT `fk_ledger_user` FOREIGN KEY (`RecordedByUserId`) REFERENCES `users` (`UserId`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_ledger_amount` CHECK ((`Amount` > 0)),
  CONSTRAINT `chk_ledger_type` CHECK ((`EntryType` in (_utf8mb4'expense',_utf8mb4'income')))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `financialledger`
--

LOCK TABLES `financialledger` WRITE;
/*!40000 ALTER TABLE `financialledger` DISABLE KEYS */;
/*!40000 ALTER TABLE `financialledger` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventories`
--

DROP TABLE IF EXISTS `inventories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventories` (
  `InventoryId` int NOT NULL AUTO_INCREMENT,
  `ProductId` int NOT NULL,
  `QuantityOnHand` int NOT NULL DEFAULT '0',
  `ReservedQuantity` int NOT NULL DEFAULT '0',
  `AvailableQuantity` int GENERATED ALWAYS AS ((`QuantityOnHand` - `ReservedQuantity`)) STORED,
  `SafetyStockLevel` int NOT NULL DEFAULT '3',
  `StorageLocation` varchar(100) DEFAULT NULL,
  `LastRestockedAt` datetime DEFAULT NULL,
  `UpdatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`InventoryId`),
  UNIQUE KEY `uq_inventories_product` (`ProductId`),
  CONSTRAINT `fk_inv_product` FOREIGN KEY (`ProductId`) REFERENCES `products` (`ProductId`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_inv_qoh` CHECK ((`QuantityOnHand` >= 0)),
  CONSTRAINT `chk_inv_rsv` CHECK ((`ReservedQuantity` >= 0)),
  CONSTRAINT `chk_inv_safety` CHECK ((`SafetyStockLevel` >= 0))
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventories`
--

LOCK TABLES `inventories` WRITE;
/*!40000 ALTER TABLE `inventories` DISABLE KEYS */;
INSERT INTO `inventories` (`InventoryId`, `ProductId`, `QuantityOnHand`, `ReservedQuantity`, `SafetyStockLevel`, `StorageLocation`, `LastRestockedAt`, `UpdatedAt`) VALUES (1,1,15,2,3,'Aisle-A-01','2026-10-14 17:30:13','2026-10-14 17:30:13');
/*!40000 ALTER TABLE `inventories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `labstudent`
--

DROP TABLE IF EXISTS `labstudent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `labstudent` (
  `StdId` varchar(10) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `StdPassword` varchar(30) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `StdName` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `StdLastname` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  PRIMARY KEY (`StdId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `labstudent`
--

LOCK TABLES `labstudent` WRITE;
/*!40000 ALTER TABLE `labstudent` DISABLE KEYS */;
INSERT INTO `labstudent` VALUES ('001','gg','ab','bd'),('002','123456','a','b');
/*!40000 ALTER TABLE `labstudent` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `preorderitems`
--

DROP TABLE IF EXISTS `preorderitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `preorderitems` (
  `ItemId` int NOT NULL AUTO_INCREMENT,
  `PreorderId` int NOT NULL,
  `ProductId` int NOT NULL,
  `UnitPrice` decimal(10,2) NOT NULL,
  `Quantity` int NOT NULL DEFAULT '1',
  `LineTotal` decimal(10,2) GENERATED ALWAYS AS ((`UnitPrice` * `Quantity`)) STORED,
  PRIMARY KEY (`ItemId`),
  KEY `fk_poi_preorder` (`PreorderId`),
  KEY `fk_poi_product` (`ProductId`),
  CONSTRAINT `fk_poi_preorder` FOREIGN KEY (`PreorderId`) REFERENCES `preorders` (`PreorderId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_poi_product` FOREIGN KEY (`ProductId`) REFERENCES `products` (`ProductId`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_poi_price` CHECK ((`UnitPrice` >= 0)),
  CONSTRAINT `chk_poi_qty` CHECK ((`Quantity` > 0))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `preorderitems`
--

LOCK TABLES `preorderitems` WRITE;
/*!40000 ALTER TABLE `preorderitems` DISABLE KEYS */;
/*!40000 ALTER TABLE `preorderitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `preorders`
--

DROP TABLE IF EXISTS `preorders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `preorders` (
  `PreorderId` int NOT NULL AUTO_INCREMENT,
  `ReservationNumber` varchar(30) NOT NULL,
  `UserId` int NOT NULL,
  `ContactPhone` varchar(20) NOT NULL,
  `EstimatedPickupDate` date NOT NULL,
  `CurrentStatus` varchar(50) NOT NULL DEFAULT 'PendingConfirm',
  `TotalFinalPrice` decimal(10,2) NOT NULL DEFAULT '0.00',
  `PickupBranch` varchar(100) NOT NULL DEFAULT 'FriendHouse HQ',
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CompletedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`PreorderId`),
  UNIQUE KEY `uq_preorders_resno` (`ReservationNumber`),
  KEY `idx_preorders_phone` (`ContactPhone`),
  KEY `idx_preorders_status` (`CurrentStatus`,`EstimatedPickupDate`),
  KEY `fk_preorders_user` (`UserId`),
  CONSTRAINT `fk_preorders_user` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_po_final` CHECK ((`TotalFinalPrice` >= 0)),
  CONSTRAINT `chk_po_status` CHECK ((`CurrentStatus` in (_utf8mb4'PendingConfirm',_utf8mb4'WaitingStock',_utf8mb4'ReadyForPickup',_utf8mb4'Completed',_utf8mb4'Cancelled')))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `preorders`
--

LOCK TABLES `preorders` WRITE;
/*!40000 ALTER TABLE `preorders` DISABLE KEYS */;
/*!40000 ALTER TABLE `preorders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `ProductId` int NOT NULL AUTO_INCREMENT,
  `Sku` varchar(50) NOT NULL,
  `Barcode` varchar(50) DEFAULT NULL,
  `Name` varchar(200) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `CategoryId` int NOT NULL,
  `SeriesName` varchar(150) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `BrandName` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `Scale` varchar(30) DEFAULT NULL,
  `HeightCm` decimal(6,2) DEFAULT NULL,
  `Material` varchar(100) DEFAULT NULL,
  `RetailPrice` decimal(10,2) NOT NULL,
  `PreorderPrice` decimal(10,2) DEFAULT NULL,
  `ExpectedArrival` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `IsPreorder` tinyint(1) NOT NULL DEFAULT '0',
  `PreorderDeadline` datetime DEFAULT NULL,
  `ImageUrl` varchar(255) NOT NULL,
  `Model3DPath` varchar(255) DEFAULT NULL,
  `IsFeaturedHero` tinyint(1) NOT NULL DEFAULT '0',
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ProductId`),
  UNIQUE KEY `uq_products_sku` (`Sku`),
  UNIQUE KEY `uq_products_barcode` (`Barcode`),
  KEY `idx_products_category` (`CategoryId`,`IsActive`),
  KEY `idx_products_preorder` (`IsPreorder`,`IsActive`),
  KEY `idx_products_featured` (`IsFeaturedHero`),
  CONSTRAINT `fk_products_category` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`CategoryId`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_products_height` CHECK (((`HeightCm` > 0) or (`HeightCm` is null))),
  CONSTRAINT `chk_products_retail` CHECK ((`RetailPrice` >= 0))
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,'FIG-NARUTO-001','8859123456789','นารูโตะ อุซึมากิ โหมดคิวบิเซจ — Ultimate Ninja Storm',1,'Naruto Shippuden','FriendHouse Studio x Bandai','1/6 Scale',32.00,'PVC + เรซิ่นคุณภาพสูง',4290.00,3990.00,'พร้อมส่งมอบทันที',0,NULL,'~/media/images/naruto_hero_fig.png','~/media/models/naruto_kyubi_sage.glb',1,1,'2026-10-14 17:30:13');
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `promotions`
--

DROP TABLE IF EXISTS `promotions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `promotions` (
  `PromotionId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(150) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `VoucherCode` varchar(50) NOT NULL,
  `PromoType` varchar(50) NOT NULL,
  `DiscountValue` decimal(10,2) DEFAULT NULL,
  `MinimumSpend` decimal(10,2) DEFAULT '0.00',
  `StartDate` date NOT NULL,
  `EndDate` date NOT NULL,
  `ApplicableScope` varchar(50) NOT NULL DEFAULT 'all',
  `UsageLimit` int DEFAULT NULL,
  `CurrentUsageCount` int NOT NULL DEFAULT '0',
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`PromotionId`),
  UNIQUE KEY `uq_promotions_voucher` (`VoucherCode`),
  KEY `idx_promotions_dates` (`StartDate`,`EndDate`),
  CONSTRAINT `chk_promo_dates` CHECK ((`EndDate` >= `StartDate`)),
  CONSTRAINT `chk_promo_discount` CHECK (((`DiscountValue` >= 0) or (`DiscountValue` is null))),
  CONSTRAINT `chk_promo_minspend` CHECK (((`MinimumSpend` >= 0) or (`MinimumSpend` is null))),
  CONSTRAINT `chk_promo_scope` CHECK ((`ApplicableScope` in (_utf8mb4'all',_utf8mb4'clearance',_utf8mb4'figures'))),
  CONSTRAINT `chk_promo_type` CHECK ((`PromoType` in (_utf8mb4'percent',_utf8mb4'fixed',_utf8mb4'bogo',_utf8mb4'bundle',_utf8mb4'double_points'))),
  CONSTRAINT `chk_promo_usage_cnt` CHECK ((`CurrentUsageCount` >= 0)),
  CONSTRAINT `chk_promo_usage_lmt` CHECK (((`UsageLimit` > 0) or (`UsageLimit` is null)))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `promotions`
--

LOCK TABLES `promotions` WRITE;
/*!40000 ALTER TABLE `promotions` DISABLE KEYS */;
/*!40000 ALTER TABLE `promotions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `receiptitems`
--

DROP TABLE IF EXISTS `receiptitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `receiptitems` (
  `ReceiptItemId` int NOT NULL AUTO_INCREMENT,
  `ReceiptId` int NOT NULL,
  `ProductId` int DEFAULT NULL,
  `ItemDescription` varchar(200) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Quantity` int NOT NULL DEFAULT '1',
  `UnitPrice` decimal(10,2) NOT NULL,
  `LineTotal` decimal(10,2) GENERATED ALWAYS AS ((`UnitPrice` * `Quantity`)) STORED,
  PRIMARY KEY (`ReceiptItemId`),
  KEY `fk_ri_receipt` (`ReceiptId`),
  KEY `fk_ri_product` (`ProductId`),
  CONSTRAINT `fk_ri_product` FOREIGN KEY (`ProductId`) REFERENCES `products` (`ProductId`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_ri_receipt` FOREIGN KEY (`ReceiptId`) REFERENCES `receipts` (`ReceiptId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `chk_ri_price` CHECK ((`UnitPrice` >= 0)),
  CONSTRAINT `chk_ri_qty` CHECK ((`Quantity` > 0))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `receiptitems`
--

LOCK TABLES `receiptitems` WRITE;
/*!40000 ALTER TABLE `receiptitems` DISABLE KEYS */;
/*!40000 ALTER TABLE `receiptitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `receipts`
--

DROP TABLE IF EXISTS `receipts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `receipts` (
  `ReceiptId` int NOT NULL AUTO_INCREMENT,
  `ReceiptNo` varchar(40) NOT NULL,
  `PreorderId` int DEFAULT NULL,
  `CustomerId` int DEFAULT NULL,
  `CustomerPhone` varchar(20) DEFAULT NULL,
  `CashierUserId` int NOT NULL,
  `SubtotalAmount` decimal(10,2) NOT NULL,
  `DiscountAmount` decimal(10,2) NOT NULL DEFAULT '0.00',
  `VatAmount` decimal(10,2) NOT NULL,
  `NetTotalAmount` decimal(10,2) NOT NULL,
  `BarcodeData` varchar(100) NOT NULL,
  `IssuedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ReceiptId`),
  UNIQUE KEY `uq_receipts_no` (`ReceiptNo`),
  KEY `idx_receipts_issued` (`IssuedAt`),
  KEY `idx_receipts_cust` (`CustomerId`),
  KEY `fk_rcpt_preorder` (`PreorderId`),
  KEY `fk_rcpt_cashier` (`CashierUserId`),
  CONSTRAINT `fk_rcpt_cashier` FOREIGN KEY (`CashierUserId`) REFERENCES `users` (`UserId`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `fk_rcpt_customer` FOREIGN KEY (`CustomerId`) REFERENCES `users` (`UserId`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_rcpt_preorder` FOREIGN KEY (`PreorderId`) REFERENCES `preorders` (`PreorderId`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `chk_rcpt_discount` CHECK ((`DiscountAmount` >= 0)),
  CONSTRAINT `chk_rcpt_net` CHECK ((`NetTotalAmount` >= 0)),
  CONSTRAINT `chk_rcpt_subtotal` CHECK ((`SubtotalAmount` >= 0)),
  CONSTRAINT `chk_rcpt_vat` CHECK ((`VatAmount` >= 0))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `receipts`
--

LOCK TABLES `receipts` WRITE;
/*!40000 ALTER TABLE `receipts` DISABLE KEYS */;
/*!40000 ALTER TABLE `receipts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rewardredemptions`
--

DROP TABLE IF EXISTS `rewardredemptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rewardredemptions` (
  `RedemptionId` bigint NOT NULL AUTO_INCREMENT,
  `RedemptionCode` varchar(40) NOT NULL,
  `UserId` int NOT NULL,
  `RewardId` int NOT NULL,
  `PointsSpent` int NOT NULL,
  `Status` varchar(30) NOT NULL DEFAULT 'PendingPickup',
  `RedeemedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CompletedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`RedemptionId`),
  UNIQUE KEY `uq_redemptions_code` (`RedemptionCode`),
  KEY `idx_redemptions_user` (`UserId`),
  KEY `fk_rdm_reward` (`RewardId`),
  CONSTRAINT `fk_rdm_reward` FOREIGN KEY (`RewardId`) REFERENCES `rewards` (`RewardId`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `fk_rdm_user` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_rdm_points` CHECK ((`PointsSpent` > 0)),
  CONSTRAINT `chk_rdm_status` CHECK ((`Status` in (_utf8mb4'PendingPickup',_utf8mb4'Completed',_utf8mb4'Cancelled')))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rewardredemptions`
--

LOCK TABLES `rewardredemptions` WRITE;
/*!40000 ALTER TABLE `rewardredemptions` DISABLE KEYS */;
/*!40000 ALTER TABLE `rewardredemptions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rewards`
--

DROP TABLE IF EXISTS `rewards`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rewards` (
  `RewardId` int NOT NULL AUTO_INCREMENT,
  `RewardCode` varchar(30) NOT NULL,
  `Title` varchar(150) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Brand` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `CategoryTag` varchar(50) NOT NULL DEFAULT 'Merchandise',
  `PointsRequired` int NOT NULL,
  `StockQuantity` int NOT NULL DEFAULT '0',
  `ImageUrl` varchar(255) NOT NULL,
  `IsAvailable` tinyint(1) NOT NULL DEFAULT '1',
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`RewardId`),
  UNIQUE KEY `uq_rewards_code` (`RewardCode`),
  CONSTRAINT `chk_rwd_points` CHECK ((`PointsRequired` > 0)),
  CONSTRAINT `chk_rwd_stock` CHECK ((`StockQuantity` >= 0))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rewards`
--

LOCK TABLES `rewards` WRITE;
/*!40000 ALTER TABLE `rewards` DISABLE KEYS */;
/*!40000 ALTER TABLE `rewards` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `systemlogs`
--

DROP TABLE IF EXISTS `systemlogs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `systemlogs` (
  `LogId` bigint NOT NULL AUTO_INCREMENT,
  `LogCode` varchar(30) NOT NULL,
  `LogLevel` varchar(20) NOT NULL,
  `Module` varchar(100) NOT NULL,
  `Message` varchar(500) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `DetailPayload` longtext,
  `IpAddress` varchar(50) DEFAULT NULL,
  `UserId` int DEFAULT NULL,
  `Timestamp` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`LogId`),
  UNIQUE KEY `uq_logs_code` (`LogCode`),
  KEY `idx_logs_level` (`LogLevel`,`Timestamp` DESC),
  KEY `idx_logs_module` (`Module`),
  KEY `fk_logs_user` (`UserId`),
  CONSTRAINT `fk_logs_user` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `chk_logs_level` CHECK ((`LogLevel` in (_utf8mb4'Info',_utf8mb4'Warning',_utf8mb4'Error',_utf8mb4'Critical')))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `systemlogs`
--

LOCK TABLES `systemlogs` WRITE;
/*!40000 ALTER TABLE `systemlogs` DISABLE KEYS */;
/*!40000 ALTER TABLE `systemlogs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserId` int NOT NULL AUTO_INCREMENT,
  `UserCode` varchar(30) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Email` varchar(150) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Phone` varchar(20) NOT NULL,
  `FullName` varchar(150) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `AvatarUrl` varchar(255) DEFAULT NULL,
  `RoleName` varchar(30) NOT NULL DEFAULT 'Customer',
  `Points` int NOT NULL DEFAULT '0',
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `uq_users_usercode` (`UserCode`),
  UNIQUE KEY `uq_users_email` (`Email`),
  KEY `idx_users_phone` (`Phone`),
  KEY `idx_users_role_active` (`RoleName`,`IsActive`),
  CONSTRAINT `chk_users_points` CHECK ((`Points` >= 0)),
  CONSTRAINT `chk_users_role` CHECK ((`RoleName` in (_utf8mb4'Customer',_utf8mb4'Staff',_utf8mb4'Owner',_utf8mb4'ITSupport',_utf8mb4'Accountant')))
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'U20261014132600','gg@gmail.com','$2a$11$CSmSneAmr/mqK6UtUXJ0reeIO2katN3Va/omKwWhUTdXnJQt98Xx.','0123456789',NULL,'','customer',0,1,'2026-10-14 06:26:01'),(2,'U20261014140358','ggg@gmail.com','$2a$11$Rje5NBcpC1lqKiwEhKKuL.oje1HLBEoQkEQQsoM3aLkS7x.BZ8FuO','0123456788',NULL,'','customer',0,1,'2026-10-14 07:03:59'),(3,'U20261014001','customer@friendhouse.com','$2a$11$uxVOjb9ddTlW7b4lRQijyOPOi1POGZxTPJmBMAfFXGZVXkJzbHmUK','081-234-5678','คุณสมชาย ใจดี (ลูกค้า)',NULL,'Customer',1200,1,'2026-10-14 17:30:14'),(4,'U20261014002','staff@friendhouse.com','$2a$11$uxVOjb9ddTlW7b4lRQijyOPOi1POGZxTPJmBMAfFXGZVXkJzbHmUK','081-234-5678','พนักงานหน้าร้าน สาขาสยาม',NULL,'Staff',1200,1,'2026-10-14 17:30:14'),(5,'U20261014003','owner@friendhouse.com','$2a$11$uxVOjb9ddTlW7b4lRQijyOPOi1POGZxTPJmBMAfFXGZVXkJzbHmUK','081-234-5678','คุณกฤษฎา เจ้าของร้าน',NULL,'Owner',1200,1,'2026-10-14 17:30:14'),(6,'U20261014004','it@friendhouse.com','$2a$11$uxVOjb9ddTlW7b4lRQijyOPOi1POGZxTPJmBMAfFXGZVXkJzbHmUK','081-234-5678','เจ้าหน้าที่ดูแลระบบ IT',NULL,'ITSupport',1200,1,'2026-10-14 17:30:14'),(7,'U20261014005','account@friendhouse.com','$2a$11$uxVOjb9ddTlW7b4lRQijyOPOi1POGZxTPJmBMAfFXGZVXkJzbHmUK','081-234-5678','ฝ่ายการเงินและบัญชี',NULL,'Accountant',1200,1,'2026-10-14 17:30:14');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-10-20  1:24:58

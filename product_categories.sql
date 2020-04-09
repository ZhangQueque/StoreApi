/*
Navicat MySQL Data Transfer

Source Server         : 阿里云RDS
Source Server Version : 80016
Source Host           : rm-8vbrz69c63749p0x5wo.mysql.zhangbei.rds.aliyuncs.com:3306
Source Database       : quequestoredb

Target Server Type    : MYSQL
Target Server Version : 80016
File Encoding         : 65001

Date: 2020-04-09 18:54:50
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for product_categories
-- ----------------------------
DROP TABLE IF EXISTS `product_categories`;
CREATE TABLE `product_categories` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PId` int(11) NOT NULL,
  `SortId` int(11) NOT NULL,
  `Status` int(11) NOT NULL,
  `CreateTime` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of product_categories
-- ----------------------------
INSERT INTO `product_categories` VALUES ('1', '男装', '0', '0', '0', '2020-04-05 03:31:32.000000');
INSERT INTO `product_categories` VALUES ('2', '女装', '0', '0', '0', '2020-04-05 03:31:44.000000');
INSERT INTO `product_categories` VALUES ('3', '童装', '0', '0', '0', '2020-04-05 03:31:55.000000');
INSERT INTO `product_categories` VALUES ('4', '内衣', '0', '0', '0', '2020-04-05 03:32:04.000000');
INSERT INTO `product_categories` VALUES ('5', 'T袖', '1', '0', '0', '2020-04-05 03:32:25.000000');
INSERT INTO `product_categories` VALUES ('6', '夹克', '1', '0', '0', '2020-04-05 03:42:51.000000');
INSERT INTO `product_categories` VALUES ('7', '西服', '1', '0', '0', '2020-04-05 03:44:10.000000');
INSERT INTO `product_categories` VALUES ('8', '牛仔裤', '1', '0', '0', '2020-04-05 03:45:40.000000');
INSERT INTO `product_categories` VALUES ('9', '风衣', '1', '0', '0', '2020-04-05 03:46:38.000000');
INSERT INTO `product_categories` VALUES ('10', '汉服', '1', '0', '0', '2020-04-05 03:47:44.000000');
INSERT INTO `product_categories` VALUES ('11', 'T袖', '2', '0', '0', '2020-04-05 03:49:58.000000');
INSERT INTO `product_categories` VALUES ('12', '卫衣', '2', '0', '0', '2020-04-05 03:51:48.000000');
INSERT INTO `product_categories` VALUES ('13', '风衣', '2', '0', '0', '2020-04-05 03:53:12.000000');
INSERT INTO `product_categories` VALUES ('14', '旗袍', '2', '0', '0', '2020-04-05 03:54:36.000000');
INSERT INTO `product_categories` VALUES ('15', '牛仔裤', '2', '0', '0', '2020-04-05 03:55:51.000000');
INSERT INTO `product_categories` VALUES ('16', 'T袖', '3', '0', '0', '2020-04-05 03:58:18.000000');
INSERT INTO `product_categories` VALUES ('17', '套装', '3', '0', '0', '2020-04-05 03:59:21.000000');
INSERT INTO `product_categories` VALUES ('18', '亲子装', '3', '0', '0', '2020-04-05 03:59:43.000000');
INSERT INTO `product_categories` VALUES ('19', '肚兜', '3', '0', '0', '2020-04-05 04:00:54.000000');
INSERT INTO `product_categories` VALUES ('20', '民族风', '3', '0', '0', '2020-04-05 04:01:51.000000');
INSERT INTO `product_categories` VALUES ('21', '文胸', '4', '0', '0', '2020-04-05 04:02:51.000000');
INSERT INTO `product_categories` VALUES ('22', '女士内裤', '4', '0', '0', '2020-04-05 04:03:12.000000');
INSERT INTO `product_categories` VALUES ('23', '男士内裤', '4', '0', '0', '2020-04-05 04:03:35.000000');
INSERT INTO `product_categories` VALUES ('24', '秋衣秋裤', '4', '0', '0', '2020-04-05 04:03:52.000000');
INSERT INTO `product_categories` VALUES ('25', '睡衣', '4', '0', '0', '2020-04-05 04:04:11.000000');
INSERT INTO `product_categories` VALUES ('26', '打底衫', '4', '0', '0', '2020-04-05 04:04:32.000000');
INSERT INTO `product_categories` VALUES ('27', '毛衣', '2', '0', '0', '2020-04-05 04:09:09.000000');
INSERT INTO `product_categories` VALUES ('28', '西装', '2', '0', '0', '2020-04-05 04:09:49.000000');
INSERT INTO `product_categories` VALUES ('29', '衬衫', '1', '0', '0', '2020-04-05 04:10:54.000000');
INSERT INTO `product_categories` VALUES ('30', '古装', '0', '0', '0', '2020-04-05 04:11:46.000000');
INSERT INTO `product_categories` VALUES ('31', 'Cosplay', '0', '0', '0', '2020-04-05 04:13:39.000000');
INSERT INTO `product_categories` VALUES ('32', '洛丽塔', '0', '0', '0', '2020-04-05 04:14:29.000000');

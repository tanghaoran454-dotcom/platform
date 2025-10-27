-- 无人机巡检检测平台数据库初始化脚本
-- 数据库：DroneInspection
-- 字符集：utf8mb4

USE DroneInspection;

-- ============================================
-- 1. 用户表
-- ============================================
CREATE TABLE IF NOT EXISTS `sys_user` (
  `Id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `Username` varchar(50) NOT NULL COMMENT '用户名',
  `Nickname` varchar(50) DEFAULT NULL COMMENT '昵称',
  `Password` varchar(100) NOT NULL COMMENT '密码（已加密）',
  `Email` varchar(100) DEFAULT NULL COMMENT '邮箱',
  `Phone` varchar(20) DEFAULT NULL COMMENT '手机号',
  `Avatar` varchar(255) DEFAULT NULL COMMENT '头像',
  `Status` int NOT NULL DEFAULT '1' COMMENT '状态（0-禁用，1-启用）',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  `IsDeleted` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否删除',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UK_Username` (`Username`),
  KEY `IDX_Status` (`Status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户表';

-- ============================================
-- 2. 角色表
-- ============================================
CREATE TABLE IF NOT EXISTS `sys_role` (
  `Id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `Name` varchar(50) NOT NULL COMMENT '角色名称',
  `Code` varchar(50) NOT NULL COMMENT '角色代码（admin-管理员，inspector-巡检员，viewer-查看员）',
  `Description` varchar(200) DEFAULT NULL COMMENT '角色描述',
  `Status` int NOT NULL DEFAULT '1' COMMENT '状态（0-禁用，1-启用）',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  `IsDeleted` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否删除',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UK_Code` (`Code`),
  KEY `IDX_Status` (`Status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='角色表';

-- ============================================
-- 3. 用户角色关联表
-- ============================================
CREATE TABLE IF NOT EXISTS `sys_user_role` (
  `Id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `UserId` bigint NOT NULL COMMENT '用户ID',
  `RoleId` bigint NOT NULL COMMENT '角色ID',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  `IsDeleted` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否删除',
  PRIMARY KEY (`Id`),
  KEY `IDX_UserId` (`UserId`),
  KEY `IDX_RoleId` (`RoleId`),
  UNIQUE KEY `UK_User_Role` (`UserId`,`RoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户角色关联表';

-- ============================================
-- 4. 权限表
-- ============================================
CREATE TABLE IF NOT EXISTS `sys_permission` (
  `Id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `Name` varchar(50) NOT NULL COMMENT '权限名称',
  `Code` varchar(100) NOT NULL COMMENT '权限代码',
  `Description` varchar(200) DEFAULT NULL COMMENT '权限描述',
  `ParentId` bigint DEFAULT NULL COMMENT '父权限ID',
  `Type` int NOT NULL COMMENT '权限类型（0-菜单，1-按钮）',
  `Sort` int NOT NULL DEFAULT '0' COMMENT '排序',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  `IsDeleted` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否删除',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UK_Code` (`Code`),
  KEY `IDX_ParentId` (`ParentId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='权限表';

-- ============================================
-- 5. 角色权限关联表
-- ============================================
CREATE TABLE IF NOT EXISTS `sys_role_permission` (
  `Id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `RoleId` bigint NOT NULL COMMENT '角色ID',
  `PermissionId` bigint NOT NULL COMMENT '权限ID',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  `IsDeleted` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否删除',
  PRIMARY KEY (`Id`),
  KEY `IDX_RoleId` (`RoleId`),
  KEY `IDX_PermissionId` (`PermissionId`),
  UNIQUE KEY `UK_Role_Permission` (`RoleId`,`PermissionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='角色权限关联表';

-- ============================================
-- 6. 初始化角色数据
-- ============================================
INSERT INTO `sys_role` (`Name`, `Code`, `Description`, `Status`) VALUES
('系统管理员', 'admin', '系统管理员，拥有所有权限', 1),
('巡检员', 'inspector', '巡检员，可以创建任务、控制设备、查看数据', 1),
('查看员', 'viewer', '查看员，只能查看数据', 1);

-- ============================================
-- 7. 初始化管理员用户
-- 密码：admin123（请使用BCrypt加密后更新）
-- ============================================
INSERT INTO `sys_user` (`Username`, `Nickname`, `Password`, `Email`, `Status`) VALUES
('admin', '系统管理员', '$2a$11$dGhpcyBpcyBhIHRlc3QgcGFzc3dvcmQ=', 'admin@example.com', 1);

-- 关联管理员角色
INSERT INTO `sys_user_role` (`UserId`, `RoleId`) 
SELECT u.Id, r.Id FROM sys_user u, sys_role r 
WHERE u.Username = 'admin' AND r.Code = 'admin';

-- ============================================
-- 8. 初始化权限数据（示例）
-- ============================================
INSERT INTO `sys_permission` (`Name`, `Code`, `Description`, `Type`, `Sort`) VALUES
('系统管理', 'system:manage', '系统管理模块', 0, 1),
('用户管理', 'system:user', '用户管理', 1, 1),
('角色管理', 'system:role', '角色管理', 1, 2),
('航线管理', 'route:manage', '航线管理', 0, 2),
('设备控制', 'device:control', '设备控制', 0, 3),
('AI识别', 'ai:recognize', 'AI识别', 0, 4),
('数据可视化', 'data:visualize', '数据可视化', 0, 5);

-- 给管理员角色分配所有权限
INSERT INTO `sys_role_permission` (`RoleId`, `PermissionId`)
SELECT r.Id, p.Id FROM sys_role r, sys_permission p
WHERE r.Code = 'admin';


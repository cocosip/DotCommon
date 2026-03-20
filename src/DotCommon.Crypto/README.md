# DotCommon.Crypto

[![NuGet](https://img.shields.io/nuget/v/DotCommon.Crypto.svg)](https://www.nuget.org/packages/DotCommon.Crypto) ![NuGet](https://img.shields.io/nuget/dt/DotCommon.Crypto.svg)

DotCommon.Crypto 是一个加密扩展包，提供 RSA、SM2、SM3、SM4 等加密算法的实现。基于 BouncyCastle.Cryptography 库构建，提供简单易用的 API。

## 特性

- **RSA 加密** - 支持密钥生成、加密/解密、签名/验签
- **SM2 国密算法** - 支持椭圆曲线加密、签名/验签
- **SM3 国密哈希** - 支持 SM3 哈希算法
- **SM4 国密对称加密** - 支持 ECB/CBC 模式
- **多种密钥格式** - 支��� PEM、PKCS#8、Base64 等多种密钥格式
- **扩展方法** - 提供便捷的字符串加密/解密扩展方法

## 安装

```bash
dotnet add package DotCommon.Crypto
```

## 快速开始

### 注册服务

```csharp
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDotCommonCrypto();

var serviceProvider = services.BuildServiceProvider();
```

## RSA 加密

RSA 是一种非对称加密算法，支持加密/解密和签名/验签功能。

### 1. 生成 RSA 密钥对

```csharp
var rsaService = serviceProvider.GetRequiredService<IRSAEncryptionService>();

// 生成 2048 位密钥对
var keyPair = rsaService.GenerateRSAKeyPair(2048);

// 获取公钥和私钥
var publicKey = keyPair.Public;
var privateKey = keyPair.Private;
```

### 2. 加密和解密

```csharp
using DotCommon.Crypto.RSA;

// 使用 PEM 格式的公钥加密
string publicKeyPem = "-----BEGIN PUBLIC KEY-----\n...\n-----END PUBLIC KEY-----";
string plainText = "Hello, World!";

// 加密（返回 Base64 字符串）
string cipherText = rsaService.EncryptFromPem(
    publicKeyPem,
    plainText,
    padding: RSAPaddingNames.PKCS1Padding
);

// 使用 PEM 格式的私钥解密
string privateKeyPem = "-----BEGIN RSA PRIVATE KEY-----\n...\n-----END RSA PRIVATE KEY-----";

// 解密
string decryptedText = rsaService.DecryptFromPem(
    privateKeyPem,
    cipherText,
    padding: RSAPaddingNames.PKCS1Padding
);
```

### 3. 签名和验签

```csharp
// 使用私钥签名
string signature = rsaService.SignFromPem(
    privateKeyPem,
    plainText,
    algorithm: "SHA256WITHRSA"
);

// 使用公钥验签
bool isValid = rsaService.VerifySignFromPem(
    publicKeyPem,
    plainText,
    signature,
    algorithm: "SHA256WITHRSA"
);

Console.WriteLine($"Signature valid: {isValid}");
```

### 4. 使用 Base64 编码的密钥

```csharp
// 使用 Base64 编码的公钥加密
string publicKeyBase64 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA...";
string encrypted = rsaService.EncryptFromBase64(publicKeyBase64, plainText);

// 使用 Base64 编码的私钥解密
string privateKeyBase64 = "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC...";
string decrypted = rsaService.DecryptFromBase64(privateKeyBase64, encrypted);
```

### RSA 填充模式

- `RSAPaddingNames.None` - 无填充
- `RSAPaddingNames.PKCS1Padding` - PKCS#1 填充
- `RSAPaddingNames.OAEPPadding` - OAEP 填充

### RSA 签名算法

- `SHA1WITHRSA` - SHA1 哈希 + RSA 签名
- `SHA256WITHRSA` - SHA256 哈希 + RSA 签名（推荐）
- `SHA384WITHRSA` - SHA384 哈希 + RSA 签名
- `SHA512WITHRSA` - SHA512 哈希 + RSA 签名

## SM2 国密加密

SM2 是中国国家密码标准，基于椭圆曲线密码学（ECC）的非对称加密算法。

### 1. 生成 SM2 密钥对

```csharp
var sm2Service = serviceProvider.GetRequiredService<ISm2EncryptionService>();

// 生成 SM2 密钥对（使用 sm2p256v1 曲线）
var keyPair = sm2Service.GenerateSm2KeyPair();

var publicKey = keyPair.Public;
var privateKey = keyPair.Private;
```

### 2. SM2 加密和解密

```csharp
using DotCommon.Crypto.SM2;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

// 公钥和私钥（十六进制字符串）
string publicKeyHex = "04B9C9A6E04E9C91F7BA880429273747D7EF5DDEB0BB2FF6317EB00BEF331...";
string privateKeyHex = "128B2FA8BD433C6C068C8D803DFF79792A519A55171B1B650C23661D15897263";
string plainText = "Hello, SM2!";

// 加密（返回十六进制字符串）
string cipherText = sm2Service.Encrypt(
    publicKeyHex,
    plainText,
    mode: Mode.C1C2C3
);

// 解密
string decryptedText = sm2Service.Decrypt(
    privateKeyHex,
    cipherText,
    mode: Mode.C1C2C3
);
```

### 3. SM2 签名和验签

```csharp
// 使用私钥签名
string signature = sm2Service.Sign(privateKeyHex, plainText);

// 使用公钥验签
bool isValid = sm2Service.VerifySign(publicKeyHex, plainText, signature);

Console.WriteLine($"Signature valid: {isValid}");
```

### 4. SM2 密文格式转换

SM2 加密结果有两种格式：C1C2C3 和 C1C3C2。

```csharp
// C1C2C3 转 C1C3C2
byte[] c1c2c3 = /* ... */;
byte[] c1c3c2 = sm2Service.C123ToC132(c1c2c3);

// C1C3C2 转 C1C2C3
byte[] c1c2c3Again = sm2Service.C132ToC123(c1c3c2);
```

### SM2 加密模式

- `Mode.C1C2C3` - C1C2C3 格式（默认）
- `Mode.C1C3C2` - C1C3C2 格式

### SM2 曲线

- `Sm2EncryptionNames.CurveSm2p256v1` - SM2 推荐曲线（默认）

## SM3 国密哈希

SM3 是中国国家密码标准的哈希算法，输出 256 位（32 字节）哈希值。

### 使用 SM3 哈希

```csharp
var sm3Service = serviceProvider.GetRequiredService<ISm3EncryptionService>();

string plainText = "Hello, SM3!";

// 计算 SM3 哈希（返回十六进制字符串）
string hash = sm3Service.GetHash(plainText);

Console.WriteLine($"SM3 Hash: {hash}");
```

### 使用字节数组

```csharp
using System.Text;

byte[] data = Encoding.UTF8.GetBytes("Hello, SM3!");
byte[] hashBytes = sm3Service.GetHash(data);

// 转换为十六进制字符串
string hashHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
```

## SM4 国密对称加密

SM4 是中国国家密码标准的对称加密算法，密钥长度为 128 位（16 字节）。

### 1. SM4 加密和解密

```csharp
using DotCommon.Crypto.SM4;

var sm4Service = serviceProvider.GetRequiredService<ISm4EncryptionService>();

string plainText = "Hello, SM4!";
string keyHex = "0123456789ABCDEFFEDCBA9876543210"; // 16 字节密钥（十六进制）
string ivHex = "0123456789ABCDEFFEDCBA9876543210";  // 16 字节 IV（十六进制）

// 使用 ECB 模式加密
string cipherText = sm4Service.Encrypt(
    plainText,
    keyHex,
    ivHex,
    mode: Sm4EncryptionNames.ModeECB,
    padding: Sm4EncryptionNames.PKCS7Padding
);

// 解密
string decryptedText = sm4Service.Decrypt(
    cipherText,
    keyHex,
    ivHex,
    mode: Sm4EncryptionNames.ModeECB,
    padding: Sm4EncryptionNames.PKCS7Padding
);
```

### 2. SM4 CBC 模式

```csharp
// 使用 CBC 模式加密
string cipherTextCBC = sm4Service.Encrypt(
    plainText,
    keyHex,
    ivHex,
    mode: Sm4EncryptionNames.ModeCBC,
    padding: Sm4EncryptionNames.PKCS7Padding
);

// 解密
string decryptedTextCBC = sm4Service.Decrypt(
    cipherTextCBC,
    keyHex,
    ivHex,
    mode: Sm4EncryptionNames.ModeCBC,
    padding: Sm4EncryptionNames.PKCS7Padding
);
```

### SM4 加密模式

- `Sm4EncryptionNames.ModeECB` - ECB 模式（默认）
- `Sm4EncryptionNames.ModeCBC` - CBC 模式

### SM4 填充方式

- `Sm4EncryptionNames.NoPadding` - 无填充（默认）
- `Sm4EncryptionNames.PKCS7Padding` - PKCS7 填充

## 配置选项

### SM2 配置

```csharp
services.Configure<DotCommonSm2EncryptionOptions>(options =>
{
    options.DefaultCurve = Sm2EncryptionNames.CurveSm2p256v1;
});
```

### SM4 配置

```csharp
using System.Text;

services.Configure<DotCommonSm4EncryptionOptions>(options =>
{
    options.DefaultIv = Encoding.UTF8.GetBytes("YourDefaultIV16");
    options.DefaultMode = Sm4EncryptionNames.ModeCBC;
    options.DefaultPadding = Sm4EncryptionNames.PKCS7Padding;
});
```

## 完整示例

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using DotCommon.Crypto.RSA;
using DotCommon.Crypto.SM2;
using DotCommon.Crypto.SM3;
using DotCommon.Crypto.SM4;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

// 配置服务
var services = new ServiceCollection();
services.AddDotCommonCrypto();
var serviceProvider = services.BuildServiceProvider();

// RSA 示例
var rsaService = serviceProvider.GetRequiredService<IRSAEncryptionService>();
var rsaKeyPair = rsaService.GenerateRSAKeyPair(2048);
Console.WriteLine("RSA 密钥对生成成功");

// SM2 示例
var sm2Service = serviceProvider.GetRequiredService<ISm2EncryptionService>();
var sm2KeyPair = sm2Service.GenerateSm2KeyPair();
Console.WriteLine("SM2 密钥对生成成功");

// SM3 示例
var sm3Service = serviceProvider.GetRequiredService<ISm3EncryptionService>();
string hash = sm3Service.GetHash("Hello, World!");
Console.WriteLine($"SM3 Hash: {hash}");

// SM4 示例
var sm4Service = serviceProvider.GetRequiredService<ISm4EncryptionService>();
string keyHex = "0123456789ABCDEFFEDCBA9876543210";
string ivHex = "0123456789ABCDEFFEDCBA9876543210";
string encrypted = sm4Service.Encrypt(
    "Hello, SM4!",
    keyHex,
    ivHex,
    Sm4EncryptionNames.ModeECB,
    Sm4EncryptionNames.PKCS7Padding
);
Console.WriteLine($"SM4 加密结果: {encrypted}");
```

## 依赖项

- BouncyCastle.Cryptography (>= 2.6.2)
- DotCommon

## 许可证

本项目采用 MIT 许可证

## 相关链接

- [DotCommon 主仓库](https://github.com/cocosip/DotCommon)
- [NuGet 包](https://www.nuget.org/packages/DotCommon.Crypto)

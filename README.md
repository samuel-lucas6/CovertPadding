[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/samuel-lucas6/CovertPadding/blob/main/LICENSE)

# CovertPadding

An implementation of the [Covert Encryption](https://github.com/covert-encryption/covert) randomised [padding scheme](https://github.com/covert-encryption/covert/blob/main/docs/Specification.md), which is intended to be [better](https://github.com/covert-encryption/covert/blob/main/docs/Security.md) than [PADMÉ](https://petsymposium.org/2019/files/papers/issue4/popets-2019-0056.pdf) padding.

The `proportion` (default of 5%) affects the amount of random padding and ensures that very small messages are at least `proportion * 500` bytes long (default of 25).

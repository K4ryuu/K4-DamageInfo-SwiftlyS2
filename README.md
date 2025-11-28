<a name="readme-top"></a>

![GitHub tag (with filter)](https://img.shields.io/github/v/tag/K4ryuu/K4-DamageInfo-SwiftlyS2?style=for-the-badge&label=Version)
![GitHub Repo stars](https://img.shields.io/github/stars/K4ryuu/K4-DamageInfo-SwiftlyS2?style=for-the-badge)
![GitHub issues](https://img.shields.io/github/issues/K4ryuu/K4-DamageInfo-SwiftlyS2?style=for-the-badge)
![GitHub](https://img.shields.io/github/license/K4ryuu/K4-DamageInfo-SwiftlyS2?style=for-the-badge)
![GitHub all releases](https://img.shields.io/github/downloads/K4ryuu/K4-DamageInfo-SwiftlyS2/total?style=for-the-badge)

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <h1 align="center">KitsuneLab©</h1>
  <h3 align="center">K4 - Damage Info</h3>
  <a align="center">A SwiftlyS2 plugin for Counter-Strike 2 that displays damage information to players. Shows real-time center screen damage indicators, console damage logs, and round-end damage summaries.</a>

  <p align="center">
    <br />
    <a href="https://github.com/K4ryuu/K4-DamageInfo-SwiftlyS2/releases/latest">Download</a>
    ·
    <a href="https://github.com/K4ryuu/K4-DamageInfo-SwiftlyS2/issues/new?assignees=K4ryuu&labels=bug&projects=&template=bug_report.md&title=%5BBUG%5D">Report Bug</a>
    ·
    <a href="https://github.com/K4ryuu/K4-DamageInfo-SwiftlyS2/issues/new?assignees=K4ryuu&labels=enhancement&projects=&template=feature_request.md&title=%5BREQ%5D">Request Feature</a>
  </p>
</div>

### Support My Work

I create free, open-source projects for the community. While not required, donations help me dedicate more time to development and support. Thank you!

<p align="center">
  <a href="https://paypal.me/k4ryuu"><img src="https://img.shields.io/badge/PayPal-00457C?style=for-the-badge&logo=paypal&logoColor=white" /></a>
  <a href="https://revolut.me/k4ryuu"><img src="https://img.shields.io/badge/Revolut-0075EB?style=for-the-badge&logo=revolut&logoColor=white" /></a>
</p>

### Dependencies

To use this server addon, you'll need the following dependencies installed:

- [**SwiftlyS2**](https://github.com/swiftly-solution/swiftlys2): SwiftlyS2 is a server plugin framework for Counter-Strike 2

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- INSTALLATION -->

## Installation

1. Install [SwiftlyS2](https://github.com/swiftly-solution/swiftlys2) on your server
2. [Download the latest release](https://github.com/K4ryuu/K4-DamageInfo-SwiftlyS2/releases/latest)
3. Extract to your server's `swiftlys2/plugins/` directory

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONFIGURATION -->

## Configuration

| Option              | Description                                             | Default |
| ------------------- | ------------------------------------------------------- | ------- |
| `RoundEndSummary`   | Show damage summary at round end                        | `true`  |
| `AllowDeathPrint`   | Allow damage summary to be shown on death               | `true`  |
| `ShowOnlyKiller`    | Show only killer in round end summary                   | `false` |
| `ShowFriendlyFire`  | Show friendly fire damage in summary                    | `false` |
| `ShowAllDamages`    | Show all damages from all players                       | `false` |
| `CenterDamageInfo`  | Show center damage info on hit                          | `true`  |
| `ConsoleDamageInfo` | Show console damage info on hit                         | `true`  |
| `FFAMode`           | Free-for-all mode (show damage to teammates)            | `false` |
| `NoRoundsMode`      | No rounds mode - clear damage on death instead of round | `false` |
| `CenterInfoTimeout` | Timeout for center damage info display (seconds)        | `3`     |

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->

## License

Distributed under the GPL-3.0 License. See [`LICENSE.md`](LICENSE.md) for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

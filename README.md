# dotnet-shell

> Be warned, this project **will** be renamed (probably to "Husk") in a future update.

## Introduction

This very simple CLI serves as a simple shell switcher. Configure your favourite terminal emulator (Terminator, cmder, ConsoleZ, Hyper, whatever) to use this as your shell and you will be prompted for what shell you *actually* want to use.

- Supports basic auto-discovery (using `--auto`)
- Supports Windows and Linux (and nominally macOS)

Future updates will include the ability to configure shells with a simple configuration file to save on messy configuration.

## Usage

Shells can be included in the prompt in two different ways:

- The `--shell` parameter can be used to manually add a shell and can be included as many times as you wish
- The `--auto` parameter will attempt to automatically detect available shells. Note that this is quite simplistic in nature (by design).

You will then be presented with a list of available shells. Navigate with the arrow keys and press Enter to launch a shell. Once exited, dotnet-shell will automatically exit (as long as `--loop` is not specified).

## Credits

Thanks to the following:

- Patrik Svensson (Spectre Systems) for the always-excellent [Spectre.Cli](https://github.com/spectresystems/spectre.cli) library
- `agolaszewski`'s Inquirer.cs library for the neat menu prompts
- `hyper-shellect` for inspiration.
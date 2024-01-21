# Cleanup Tool (StoneKit.Utilities.CleanUp)

A .NET 8 Core application designed to streamline the cleanup of .NET project directories by eliminating bin, obj, and .vs folders. 
You can execute this tool with default configurations through a double click. 
Alternatively, you have the option to provide a configuration file, use the command line, or initiate the process with specific parameters.Certainly! Here's a revised version:

## Usage

### Download

1. Download the Cleanup Tool executable from [**Here**](./publish/StoneKit.Utilities.CleanUp.exe).
2. Put it in the project or solution root directory with any name you want.

### Running the Tool

- **Default Configuration:**
Execute the tool by double-clicking the executable. It will run in interactive mode and asks for configuration options.

- **Custom Configuration:**
Create a sample configuration file using -g and customize the options. 

- **Command-line Options:**
Run the tool using the command line with various options. Example:
```bash
StoneKit.Utilities.CleanUp.exe -b -v
```

### Options

- `-b` or `--remove-bin-obj`: Remove bin and obj folders.
- `-v` or `--remove-dot-vs`: Remove .vs folders.
- `-i` or `--interactive`: Run the tool interactively.
- `-g` or `--generate-config`: Generate a default INI configuration file.
- `-h` or `--help`: Display help information.

## Contributing

If you'd like to contribute to the project, follow these steps:

1. Fork the repository.
2. Create a new branch for your feature or bug fix: `git checkout -b feature/new-feature`.
3. Make your changes and commit them: `git commit -m 'Add new feature'`.
4. Push to the branch: `git push origin feature/new-feature`.
5. Open a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

If you have questions or need further assistance, feel free to open an issue on [GitHub](https://github.com/desmati/StoneKit/issues).

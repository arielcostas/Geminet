# Geminet

A cross-platform, high performance, and easy to use gemini server written in .NET.

This is not a production ready server. It is still in development and is not yet feature complete.

The goal of this project is to provide a gemini server that sits in front of other sites and proxies requests to them.
Similar to how NGINX or Apache are only used to proxy requests to other web servers, run CGI scripts or serve static
files, Geminet is not an application server but rather a load balancer/reverse proxy.

## Features

- [X] Easily configurable via JSON file
- [X] Supports multiple domains/hosts
- [X] Generates a self-signed certificate for each host
- [ ] Generates directory listings for directories without an index.gmi file
- [ ] Supports generating atom feeds for certain static routes (configurable via settings)
- [ ] Supports reverse proxying
- [ ] Supports serving static files
- [ ] Supports logging requests and errors to a file
- [ ] Startup option for setting a custom config file path
- [ ] System service support
    - [ ] Windows
    - [ ] Linux (systemd)
- [ ] Docker support

## Configuration

The configuration file MUST be a JSON file in the current working directory. The file is named `server.json` and
contains the following properties:

- `certRoot`: The path to the directory where the certificates will be stored. This directory MUST exist and be
  writable.
- `certPassword`: The password to use when generating the certificates. This password is used to encrypt the private key
  and MUST be at least 4 characters long. Please note, changing this password will cause the server to be unable to
  read the existing certificates. 
- `sites`: An array of site configurations. Each site configuration contains the following properties:
    - `name`: A human readable name for the site. This is used for logging purposes.
    - `hostname`: The hostname of the site. This is used to generate the certificate for the site and serve the
      appropriate content.
    - `listen`: The port where the server will listen for requests. This MUST be a valid port number. The port will be
      listened to on all available addresses.
    - `serve`: The path to the directory where the site's content is located. This directory MUST exist and be
      readable.

## Example Configuration

```json
{
    "certRoot": "C:\\Geminet\\certs",
    "certPassword": "mYs3cr3tP@ssw0rd",
    "sites": [
        {
            "name": "Example Site",
            "hostname": "example.com",
            "listen": 1965,
            "serve": "C:\\Geminet\\sites\\example_com"
        },
        {
            "name": "Example Site 2",
            "hostname": "example2.com",
            "listen": 1965,
            "serve": "C:\\Geminet\\sites\\example2_com"
        }
    ]
}
```
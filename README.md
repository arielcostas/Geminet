# Geminet

A cross-platform, high performance, and easy to use gemini server written in .NET.

This is not a production ready server. It is still in development and is not yet feature complete.

The goal of this project is to provide a gemini server that sits in front of other sites and proxies requests to them.
Similar to how NGINX or Apache are only used to proxy requests to other web servers, run CGI scripts or serve static
files, Geminet is not an application server but rather a load balancer/reverse proxy.

## Features

- [ ] Easily configurable via INI file
- [ ] Supports multiple domains/hosts
- [ ] Generates a self-signed certificate for each host
- [ ] Generates directory listings for directories without an index.gmi file
- [ ] Supports reverse proxying
- [ ] Supports CGI scripts
- [ ] Supports serving static files
- [ ] Supports logging requests and errors to a file

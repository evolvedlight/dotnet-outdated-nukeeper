# Neukeeper

This is a reimplementation of Nukeeper using dotnet-outdated underneath.

*This project is still under development and doesn't yet support full functionality*

Supported providers:

| Provider | Support |
| -------- | ------- |
| Github   | :hammer:|
| Bitbucket Server | :hammer:|

# How to use:

## Github:
Add an environment variable REPO_TOKEN with a PAT for the repo
Then run:
`neukeeper https://github.com/evolvedlight/sample-outdated  --repo-type="github" -pr -u --username="evolvedlight" --commitEmail="steve@brown.bg"`

## BitBucket:
Add an environment variable REPO_TOKEN 
Then run:
```neukeeper http://localhost:17990/projects/TEST/repos/sample-outdated/browse --repo-type="bitbucketserver" -pr -u --username="evo" --commitEmail="steve@brown.bg"```

### Optional Arguments

#### Max packages to update

Use `max-package-updates` to control how many packages to update. For example `max-package-updates=2` will update a maximum of 2 packages
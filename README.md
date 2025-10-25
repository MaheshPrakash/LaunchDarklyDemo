# LaunchDarklyDemo

## Overview
**LaunchDarklyDemo** is a .NET REST API project demonstrating **feature flagging** using [LaunchDarkly](https://launchdarkly.com/).  
This project allows dynamic enabling or disabling of features in a running application without redeploying, using LaunchDarkly’s **server-side SDK**.

Built with **.NET 9**, it showcases feature flag evaluation per user context and includes a simple endpoint to test flags.

---

## Features
- Toggle features dynamically via the **LaunchDarkly dashboard**  
- Supports boolean feature flags (on/off)  
- Example endpoint: `/test-flag`  
  - Returns JSON with flag status (`true`/`false`)  
  - Example response:

```json
{
  "flag": "showNewUI",
  "enabled": true,
  "message": "✅ New UI is enabled!"
}

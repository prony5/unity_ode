# Open Dynamic Engine for Unity

A [ODE](https://www.ode.org/) physics for Unity.

## Installation

### Via Git URL (Recommended)
1. Open your Unity project
2. Navigate to `Window > Package Manager`
3. Click `+ > Add package from git URL`
4. Enter:  
   `https://github.com/prony5/unity_ode.git`

### Via Manifest.json
Add to your `Packages/manifest.json`:
```json
"dependencies": {
  "com.prony5.unity.ode": "https://github.com/prony5/unity_ode.git"
}
```

## Quick Start

1. **Add to Scene**:
   - Add `Ode/World` component
   - Add Bodies and joints form section  `Ode` in inspector

2. By default, the simulation is performed using single precision numbers, to use double you need to add `ODE_DOUBLE_PRECISION` in scripting define symbols in project settings.

## Requirements

| Component      | Requirement                         |
|----------------|-------------------------------------|
| Unity Version  | 2019.4 LTS or later                 |
| Platforms      | Windows, Linux                      |

## Troubleshooting

[📄 Read ODE_en.pdf](/Docs/ode_en.pdf)
[📄 Read ODE_ru.pdf](/Docs/ode_ru.pdf)

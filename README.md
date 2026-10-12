# 🥽 VR Virtual Laboratory: Characteristics of MOSFET (N-Channel)

An immersive Virtual Reality (VR) engineering experiment simulator developed in **Unity** with **XR Interaction Toolkit** and **XCharts**. This project allows students and engineers to conduct laboratory experiments to study the **Drain Characteristics** and **Transfer Characteristics** of an N-Channel Enhancement MOSFET in an interactive 3D virtual laboratory environment.

---

## 📌 Table of Contents
1. [Overview](#-overview)
2. [Experiment Objectives](#-experiment-objectives)
3. [Theory & Characteristics](#-theory--characteristics)
   - [Drain Characteristics ($I_D$ vs $V_{DS}$)](#1-drain-characteristics-i_d-vs-v_ds)
   - [Transfer Characteristics ($I_D$ vs $V_{GS}$)](#2-transfer-characteristics-i_d-vs-v_gs)
   - [MOSFET Parameters & Equations](#3-key-equations--parameters)
4. [Virtual Apparatus & Setup](#-virtual-apparatus--setup)
5. [VR Laboratory Procedure](#-vr-laboratory-procedure)
6. [Unity Architecture & Codebase](#-unity-architecture--codebase)
7. [Installation & Setup](#-installation--setup)
8. [License & Credits](#-license--credits)

---

## 🌟 Overview

Physical electronics labs require expensive instruments (dual Regulated Power Supplies, Digital Multimeters, breadboards, and sensitive semiconductor components) that can be easily damaged due to overcurrent or incorrect wiring. 

This VR project simulates a real-life lab experience where the student can:
- Grab and inspect 3D electronic components (MOSFET, resistors, breadboard, wires).
- Place and socket the MOSFET on a breadboard.
- Interact with dual Regulated Power Supplies (RPS) using interactive VR knobs and switches.
- Monitor real-time voltages ($V_{GS}, V_{DS}$) and drain current ($I_D$) on digital meters.
- Record readings dynamically to a virtual tabulation panel.
- Plot real-time characteristic curves using dynamic graph plotting in VR via XCharts.

---

## 🎯 Experiment Objectives

1. To set up the circuit for studying the characteristics of an **N-Channel Enhancement MOSFET**.
2. To obtain the **Drain Characteristics** ($I_D$ vs $V_{DS}$) for different constant values of Gate-to-Source voltage ($V_{GS}$).
3. To obtain the **Transfer Characteristics** ($I_D$ vs $V_{GS}$) for a fixed Drain-to-Source voltage ($V_{DS}$).
4. To determine parameters from the characteristics curves:
   - **Threshold Voltage ($V_{th}$)**
   - **Drain Resistance ($r_d$)**
   - **Transconductance ($g_m$)**
   - **Amplification Factor ($\mu$)**

---

## ⚡ Theory & Characteristics

A **MOSFET (Metal-Oxide-Semiconductor Field-Effect Transistor)** is a unipolar, voltage-controlled semiconductor device. In an N-channel enhancement-mode MOSFET:
- With $V_{GS} = 0\text{ V}$, the channel between Drain and Source is non-conducting ($I_D \approx 0$).
- When a positive gate voltage $V_{GS} > V_{th}$ (Threshold Voltage) is applied, electrons are attracted to the surface under the gate oxide, creating an inversion channel connecting Drain and Source.

### 1. Drain Characteristics ($I_D$ vs $V_{DS}$)
Plots the variation of Drain Current ($I_D$) with Drain-to-Source Voltage ($V_{DS}$) keeping Gate-to-Source Voltage ($V_{GS}$) constant.

The curve has three distinct regions:
1. **Ohmic / Linear Region ($V_{DS} < V_{GS} - V_{th}$):**
   - The MOSFET acts like a voltage-controlled resistor.
   - Drain current increases linearly with $V_{DS}$.
   - $I_D \approx \mu_n C_{ox} \frac{W}{L} \left[(V_{GS} - V_{th})V_{DS} - \frac{V_{DS}^2}{2}\right]$
2. **Pinch-Off / Saturation Region ($V_{DS} \ge V_{GS} - V_{th}$):**
   - The channel pinches off near the drain end.
   - Current saturates and remains nearly constant regardless of further increase in $V_{DS}$.
   - $I_D = \frac{1}{2} \mu_n C_{ox} \frac{W}{L} (V_{GS} - V_{th})^2$
3. **Breakdown Region:**
   - At very high $V_{DS}$, avalanche breakdown occurs, causing a sharp, uncontrolled surge in $I_D$.

### 2. Transfer Characteristics ($I_D$ vs $V_{GS}$)
Plots the variation of Drain Current ($I_D$) as a function of Gate-to-Source Voltage ($V_{GS}$) for a constant Drain-to-Source Voltage ($V_{DS} > V_{DS(\text{sat})}$).
- When $V_{GS} < V_{th}$: Cutoff region, $I_D = 0$.
- When $V_{GS} \ge V_{th}$: Channel is formed; $I_D$ increases quadratically with $(V_{GS} - V_{th})$.

### 3. Key Equations & Parameters

| Parameter | Symbol | Formula | Unit |
| :--- | :---: | :--- | :---: |
| **Drain Resistance** | $r_d$ | $r_d = \left.\frac{\Delta V_{DS}}{\Delta I_D}\right\|_{V_{GS} = \text{const}}$ | $\Omega$ or $\text{k}\Omega$ |
| **Transconductance** | $g_m$ | $g_m = \left.\frac{\Delta I_D}{\Delta V_{GS}}\right\|_{V_{DS} = \text{const}}$ | $\text{mA/V}$ or $\text{mS}$ |
| **Amplification Factor** | $\mu$ | $\mu = g_m \times r_d = \left.\frac{\Delta V_{DS}}{\Delta V_{GS}}\right\|_{I_D = \text{const}}$ | Dimensionless |

---

## 🔬 Virtual Apparatus & Setup

| S.No | Component / Tool in VR | Description / Function in Unity |
| :--- | :--- | :--- |
| 1 | **MOSFET Model (SreeAnjanaMOSFET.fbx)** | 3D N-Channel MOSFET model with interactive pins (Gate, Drain, Source). |
| 2 | **Dual Regulated Power Supply (RPS1 & RPS2)** | RPS1 controls $V_{GS}$ (Gate-Source bias); RPS2 controls $V_{DS}$ (Drain bias). |
| 3 | **Interactive Rotary Knobs (`RPSKnob.cs`)** | Rotatable knobs with XR grab/ray interactors to finely adjust voltages. |
| 4 | **Power Buttons (`RPSPowerButton.cs`)** | Push buttons to toggle power state on/off for power supplies. |
| 5 | **Digital Displays (TMP_Text)** | Real-time 7-segment/digital readouts for Voltages ($V_{GS}, V_{DS}$) and Current ($I_D$). |
| 6 | **Virtual Breadboard & Hole Sockets** | Socket snapping system (`BreadboardHole.cs`, `ComponentSnap.cs`). |
| 7 | **Tabulation Panel (`MOSFETValueRecorder.cs`)** | VR Table UI that records $(V_{GS}, V_{DS}, I_D)$ rows dynamically. |
| 8 | **Graph Display (`MOSFETGraphPlotter.cs`)** | Embedded line chart rendered in VR via **XCharts** plotting family of curves. |

---

## 📝 VR Laboratory Procedure

### Part A: Drain Characteristics ($I_D$ vs $V_{DS}$)
1. **Enter VR Lab Environment:** Put on VR Headset (Meta Quest / HTC Vive / XR Simulator) and approach the lab workbench.
2. **Mount Component:** Grab the MOSFET and snap it into the designated breadboard socket.
3. **Turn ON RPS units:** Press the green power buttons on RPS 1 and RPS 2.
4. **Set Constant $V_{GS}$:**
   - Rotate the RPS 1 knob until $V_{GS}$ reads **3.5 V** on Voltmeter 1.
5. **Vary $V_{DS}$ and Record $I_D$:**
   - Rotate the RPS 2 knob in calibrated steps (e.g., $1\text{V}, 3\text{V}, 5\text{V}, 7\text{V}, 9\text{V}, 11\text{V}, 15\text{V}, 18\text{V}, 23\text{V}, 25\text{V}$).
   - Press the VR **"Record Value"** button at each step to log values into the table.
6. **Repeat for other $V_{GS}$ values:**
   - Change $V_{GS}$ to **3.6 V** and repeat the variation of $V_{DS}$.
   - Change $V_{GS}$ to **3.7 V** and repeat the variation of $V_{DS}$.
7. **Plot Graph:** Press the **"Plot Graph"** button to generate the family of drain characteristic curves in real time.

### Part B: Calculating Parameters
1. **Drain Resistance ($r_d$):**
   - In the saturation region of the $V_{GS} = 3.6\text{ V}$ curve, select two points $(V_{DS1}, I_{D1})$ and $(V_{DS2}, I_{D2})$.
   - Compute: $r_d = \frac{V_{DS2} - V_{DS1}}{I_{D2} - I_{D1}}$.
2. **Transconductance ($g_m$):**
   - For a constant $V_{DS}$ (e.g., $15\text{ V}$), find the difference in $I_D$ between $V_{GS} = 3.7\text{ V}$ and $V_{GS} = 3.5\text{ V}$.
   - Compute: $g_m = \frac{I_{D(3.7\text{V})} - I_{D(3.5\text{V})}}{3.7\text{V} - 3.5\text{V}}$.
3. **Amplification Factor ($\mu$):**
   - Compute: $\mu = g_m \times r_d$.

---

## 💻 Unity Architecture & Codebase

The logic is modularized across clean C# scripts located in [`Assets/scritps/`](file:///d:/Unity%20projects/vr%20lab/Assets/scritps):

| Script | Purpose & Functionality |
| :--- | :--- |
| [`CircuitController.cs`](file:///d:/Unity%20projects/vr%20lab/Assets/scritps/CircuitController.cs) | Calculates circuit currents and voltages using Ohm's Law and drives digital readouts. |
| [`MOSFETValueRecorder.cs`](file:///d:/Unity%20projects/vr%20lab/Assets/scritps/MOSFETValueRecorder.cs) | Models MOSFET piecewise non-linear drain curves for reference gate biases and records rows to VR UI table. |
| [`MOSFETGraphPlotter.cs`](file:///d:/Unity%20projects/vr%20lab/Assets/scritps/MOSFETGraphPlotter.cs) | Interfaces with **XCharts** runtime to dynamically plot series for $V_{GS} = 3.5\text{V}, 3.6\text{V}, 3.7\text{V}$. |
| [`RPSController.cs`](file:///d:/Unity%20projects/vr%20lab/Assets/scritps/RPSController.cs) | Regulated Power Supply state management (Power toggle, voltage/current scaling). |
| [`RPSKnob.cs`](file:///d:/Unity%20projects/vr%20lab/Assets/scritps/RPSKnob.cs) | Interactor logic converting XR grab/rotation deltas into analog voltage adjustments. |
| [`RPSPowerButton.cs`](file:///d:/Unity%20projects/vr%20lab/Assets/scritps/RPSPowerButton.cs) | Button click handler triggering RPS power state. |
| [`BreadboardHoleGenerator.cs`](file:///d:/Unity%20projects/vr%20lab/Assets/TEST%20SCRIPTS/BreadboardHoleGenerator.cs) | Procedurally generates coordinate-addressed breadboard contact holes. |
| [`ComponentSnap.cs`](file:///d:/Unity%20projects/vr%20lab/Assets/TEST%20SCRIPTS/ComponentSnap.cs) | Snaps pins of components into breadboard hole matrices with haptic and visual feedback. |

---

## 🛠️ Installation & Setup

### Prerequisites
- **Unity Editor:** 2022.3 LTS or 2021.3 LTS
- **Render Pipeline:** Universal Render Pipeline (URP)
- **VR Plugin:** Unity XR Interaction Toolkit (v2.x+), OpenXR Plugin
- **Graphing Library:** XCharts (included in project)
- **Git LFS:** Git Large File Storage must be enabled for large 3D models (`*.fbx`).

### Clone & Open
```bash
# 1. Clone repository
git clone <YOUR_REPOSITORY_URL> "vr lab"
cd "vr lab"

# 2. Pull Git LFS objects (for 3D models like ROOM.fbx)
git lfs pull

# 3. Open in Unity Hub
# Click 'Add project from disk' -> Select this folder
# Open Scene: Assets/Scenes/main project.unity
```

---

## 📄 License & Credits
Developed for immersive engineering education and virtual STEM laboratories.
Designed by **SingleHari** & Contributors.

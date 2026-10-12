# 📘 Laboratory Manual & Notes: Characteristics of MOSFET in VR

## Experiment Name
**Study and Verification of Drain and Transfer Characteristics of an N-Channel MOSFET in Virtual Reality Environment**

---

## 1. AIM OF THE EXPERIMENT
1. To obtain and analyze the **Drain Characteristics** ($I_D$ versus $V_{DS}$) of an N-Channel enhancement MOSFET for different constant values of Gate-to-Source Voltage ($V_{GS}$).
2. To plot the **Transfer Characteristics** ($I_D$ versus $V_{GS}$) of the MOSFET for a fixed value of Drain-to-Source Voltage ($V_{DS}$).
3. To calculate the dynamic parameters:
   - Drain Resistance ($r_d$)
   - Transconductance ($g_m$)
   - Amplification Factor ($\mu$)
   - Threshold Voltage ($V_{th}$)

---

## 2. APPARATUS & COMPONENTS (VR WORKSPACE)
| Component | Specification / Type | Quantity | Function in Simulation |
| :--- | :--- | :---: | :--- |
| **MOSFET** | N-Channel Enhancement Type (IRF Series) | 1 | Device Under Test (DUT) |
| **Dual Regulated Power Supply (RPS)** | 0 – 30V DC variable with fine adjust knobs | 2 | RPS1: $V_{GS}$ bias, RPS2: $V_{DS}$ bias |
| **Resistors** | $220\,\Omega$, $3.3\,\text{k}\Omega$ | 1 each | Current limiting / biasing |
| **Digital Voltmeters** | $0 - 30\text{ V}$ digital readout | 2 | Displays $V_{GS}$ and $V_{DS}$ |
| **Digital Milliammeter** | $0 - 100\text{ mA}$ digital readout | 1 | Displays Drain Current ($I_D$) |
| **Virtual Breadboard & Wires** | Snap socket matrix with color-coded wire line renderer | 1 set | Circuit assembly in 3D |
| **Virtual Recording Panel & XChart** | VR UI interactive canvas | 1 | Data logging & real-time plotting |

---

## 3. CIRCUIT DIAGRAM DESCRIPTION
```text
          +------------- (mA) ------------+
          |            Ammeter            |
          |                               |
       +-----+                          +-+----+
       | RPS | (VDS)                   |  D   |
       |  2  |                         |MOSFET|
       +-----+                         |  G   S|
          |                             +-+----+
          |                               |    |
         GND -----------------------------+----+---------------- GND
                                               |
                                            +-----+
                                            | RPS | (VGS)
                                            |  1  |
                                            +-----+
```
- **Gate Terminal (G):** Connected to RPS 1 positive terminal through limiting resistor. Measures $V_{GS}$.
- **Drain Terminal (D):** Connected to RPS 2 positive terminal through Milliammeter. Measures $V_{DS}$ and $I_D$.
- **Source Terminal (S):** Connected to common ground along with negative terminals of RPS 1 and RPS 2.

---

## 4. FORMULAS & CALCULATION REFERENCE

### A. Dynamic Drain Resistance ($r_d$)
The ratio of incremental change in drain-to-source voltage to the corresponding incremental change in drain current at a constant gate-to-source voltage:
$$r_d = \left( \frac{\Delta V_{DS}}{\Delta I_D} \right)_{V_{GS} = \text{constant}} \quad (\text{measured in } \Omega \text{ or } \text{k}\Omega)$$

### B. Mutual Conductance / Transconductance ($g_m$)
The ratio of incremental change in drain current to the corresponding incremental change in gate-to-source voltage at a constant drain-to-source voltage:
$$g_m = \left( \frac{\Delta I_D}{\Delta V_{GS}} \right)_{V_{DS} = \text{constant}} \quad (\text{measured in } \text{mA/V} \text{ or } \text{mS})$$

### C. Amplification Factor ($\mu$)
$$\mu = g_m \times r_d = \left( \frac{\Delta V_{DS}}{\Delta V_{GS}} \right)_{I_D = \text{constant}}$$

---

## 5. TABULAR COLUMN & OBSERVATION DATA

### Observation Table: Drain Characteristics ($I_D$ vs $V_{DS}$)

| S.No | $V_{DS}$ (Volts) | $I_D$ (mA) for $V_{GS} = 3.5\text{V}$ | $I_D$ (mA) for $V_{GS} = 3.6\text{V}$ | $I_D$ (mA) for $V_{GS} = 3.7\text{V}$ |
| :---: | :---: | :---: | :---: | :---: |
| 1 | 1.0 | 0.2 | 0.3 | 0.3 |
| 2 | 3.0 | 0.9 | 0.9 | 0.9 |
| 3 | 5.0 | 0.9 | 1.6 | 1.6 |
| 4 | 7.0 | 0.9 | 2.2 | 2.2 |
| 5 | 9.0 | 0.9 | 2.6 | 2.7 |
| 6 | 11.0 | 0.9 | 2.6 | 3.4 |
| 7 | 15.0 | 0.9 | 2.6 | 4.6 |
| 8 | 18.0 | 0.9 | 2.6 | 5.2 |
| 9 | 23.0 | 0.9 | 2.6 | 5.4 |
| 10 | 25.0 | 0.9 | 2.6 | 5.8 |

---

## 6. GRAPH PLOTTING GUIDELINES
1. **Drain Characteristics Curve:**
   - X-Axis: $V_{DS}$ (Drain to Source Voltage in Volts)
   - Y-Axis: $I_D$ (Drain Current in mA)
   - Plot 3 curves corresponding to $V_{GS} = 3.5\text{V}$, $V_{GS} = 3.6\text{V}$, and $V_{GS} = 3.7\text{V}$.
   - Notice the initial linear ohmic increase followed by horizontal plateau (saturation region).

2. **Transfer Characteristics Curve:**
   - X-Axis: $V_{GS}$ (Gate to Source Voltage in Volts)
   - Y-Axis: $I_D$ (Drain Current in mA) at a constant $V_{DS}$ (e.g., $15\text{V}$).
   - Notice the intercept on the X-axis where $I_D$ begins to rise; this intercept represents the **Threshold Voltage ($V_{th} \approx 3.45\text{V}$)**.

---

## 7. VR LAB INTERACTION GUIDE
- **VR Ray / Grab:** Point XR Ray at the RPS knob and grip with trigger/grip button to rotate voltage clockwise/counter-clockwise.
- **Record Button:** Triggers `MOSFETValueRecorder.RecordValue()` which pushes a new row into the in-game table canvas.
- **Plot Graph Button:** Invokes `MOSFETGraphPlotter.PlotGraph()` which renders smooth splines via XCharts inside the virtual room.
- **Clear Button:** Clears table and resets graph series.

---

## 8. RESULT
1. The Drain and Transfer characteristics of the N-Channel MOSFET have been successfully obtained in the VR Laboratory environment.
2. The dynamic parameters calculated from the characteristics:
   - Drain Resistance $r_d =$ ____________ $\Omega$
   - Transconductance $g_m =$ ____________ $\text{mA/V}$
   - Amplification Factor $\mu =$ ____________
   - Threshold Voltage $V_{th} =$ ____________ $\text{V}$

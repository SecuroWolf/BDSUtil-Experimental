# BDSUtil

**Disclaimer:** BDSUtil is a personal project and is not affiliated with, endorsed by, or associated with Mojang Studios or Microsoft Corporation. Minecraft, Bedrock Edition, and BDS (Bedrock Dedicated Server) are trademarks or registered trademarks of their respective owners.


THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.

IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE 
USE OR OTHER DEALINGS IN THE SOFTWARE.

WITHOUT LIMITING THE FOREGOING, I ACCEPT ABSOLUTELY NO LIABILITY 
IF THIS SOFTWARE CORRUPTS YOUR DATA, CAUSES YOUR ALARM NOT TO GO OFF 
RESULTING IN YOU MISSING WORK, DESTROYS YOUR HARDWARE, OR INADVERTENTLY 
TRIGGERS A THERMONUCLEAR WAR. YOU USE THIS SOFTWARE ENTIRELY AT YOUR OWN RISK.

## Features

### 🖥️ Interactive Console & GUI Wrapper
* **Full Console Stream:** Displays real-time `bedrock_server.exe` logs directly inside the app wrapper.
* **Command Interactivity:** Dedicated input terminal allows you to execute standard console commands on the fly.
* **One-Click Power Controls:** Instant start and stop macros via floating dashboard action buttons.

### 📝 Configuration Editor
* **GUI-Based Property Management:** Easily view, toggle, and update your baseline `server.properties` and whitelist profiles straight from a clean control dashboard panel without ever needing to navigate configuration text sheets directly.

### 🌐 Dynamic UPnP Port Provisioning & Network Management
* **Network-Security-First Default:** Consistent with a secure-by-default footprint, all automated network mapping subsystems are strictly disabled out of the box. Users must explicitly toggle UPnP on within their configuration sub-panels.
* **Automated Gateway Mapping:** Once enabled, the integrated UPnP engine negotiates router firewall tables on app launch, entirely bypassing manual port forwarding requirements.
* **Dual-Protocol Base Forwarding:** Automatically detects active `server-port` and `server-portv6` assignments on boot, creating simultaneous high-performance TCP and UDP routes for primary traffic.
* **Dynamic Multi-Node Pools:** Proactively leases sequential UDP transport ports starting at `19134` to balance multiple player connections smoothly, scaling with the configured player slot constraints (up to Mojang's hard 16-port threshold).
* **Fault-Tolerant Allocation:** Automatically detects local network mapping collisions or router lease rejections, skipping blocked ports instantly to lease alternative slots until the required number is reached.
* **Manual Mapping Wizard:** For configurations where UPnP is unavailable or disabled, a simplified, streamlined Manual Port Mapping panel is built-in to allow easy tracking and administration of static forwarding nodes.
* **Self-Cleaning Lifecycle:** Intercepts application restarts and system closures with an asynchronous visual progress modal, cleanly scrubbing temporary active hardware rules from your router while returning `server.properties` to a pristine commented state.

### 💾 Snapshot, Backup & Recovery Engine
* **Granular Backups:** Target specific backups for your individual Worlds, Resource/Behavior Packs, or execute a Full Server Archive.
* **Flexible Compression:** Automatically defaults to native Windows Compression (`.zip`), with seamless support for WinRAR integration if detected in its default installation directory.
* **Snapshot Recovery:** Dedicated restoration hub to roll back worlds and files to older states easily.

### ⚙️ Portable Environment Management
* **Decoupled Server Directory:** Can function completely outside of the server folder via a manual path picker.
* **Zero-Config Detection:** Automatically populates the target directory at launch if the app file detects a local `bedrock_server.exe`.

---

## Libraries & Credits
This project leverages the following incredible open-source libraries to power its interface and networking stack:
* **[ReaLTaiizor](https://github.com/Taiizor/ReaLTaiizor):** A comprehensive, feature-rich .NET WinForms component library that powers BDSUtil's modern Material Design layout ecosystem.
* **[Open.NAT](https://github.com/lontivero/Open.NAT):** A lightweight, asynchronous .NET library used to manage port mapping allocations via UPnP and PMP seamlessly.

---

## Roadmap / Planned Features
* 💬 **Chat Filter Management:** Real-time log monitoring tab featuring configurable violation enforcements (redaction, mute, kick, warning strikes) alongside precise wildcard evaluations.
* 📦 **Addon & Pack Management:** In-app installer and manager for server additions.
* 🔄 **BDS Core Updates:** Automated and manual updating pipeline for the main server binary.

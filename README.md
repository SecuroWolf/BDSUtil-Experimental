# BDSUtil

This is a work in progress project of mine. 

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 

IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE 
USE OR OTHER DEALINGS IN THE SOFTWARE.

WITHOUT LIMITING THE FOREGOING, THE AUTHOR ACCEPTS ABSOLUTELY NO LIABILITY 
IF THIS SOFTWARE CORRUPTS YOUR DATA, CAUSES YOUR ALARM NOT TO GO OFF 
RESULTING IN YOU MISSING WORK, DESTROYS YOUR HARDWARE, OR INADVERTENTLY 
TRIGGERS A THERMONUCLEAR WAR. YOU USE THIS SOFTWARE ENTIRELY AT YOUR OWN RISK.


## Features

### 🖥️ Interactive Console & GUI Wrapper
* **Full Console Stream:** Displays real-time `bedrock_server.exe` logs directly inside the app wrapper.
* **Command Interactivity:** Dedicated input terminal allows you to execute standard console commands on the fly.
* **One-Click Power Controls:** Instant start and stop macros via floating dashboard action buttons.

### 💾 Snapshot, Backup & Recovery Engine
* **Granular Backups:** Target specific backups for your individual Worlds, Resource/Behavior Packs, or execute a Full Server Archive.
* **Flexible Compression:** Automatically defaults to native Windows Compression (`.zip`), with seamless support for WinRAR integration if detected in its default installation directory.
* **Snapshot Recovery:** Dedicated restoration hub to roll back worlds and files to older states easily.

### ⚙️ Portable Environment Management
* **Decoupled Server Directory:** Can function completely outside of the server folder via a manual path picker.
* **Zero-Config Detection:** Automatically populates the target directory at launch if the app file detects a local `bedrock_server.exe`.

---

## Roadmap / Planned Features
* 📦 **Addon & Pack Management:** In-app installer and manager for server additions.
* 🔄 **BDS Core Updates:** Automated and manual updating pipeline for the main server binary.
* 📝 **Configuration Editor:** GUI-based manager to edit `server.properties` and whitelist profiles without navigating config files.

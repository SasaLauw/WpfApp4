| Bagian             | Fungsi                                  |
| ------------------ | --------------------------------------- |
| `Window`           | Jendela utama program                   |
| `DockPanel`        | Tata letak vertikal (toolbar + kanvas)  |
| `StackPanel`       | Toolbar dengan tombol dan pemilih warna |
| `Canvas`           | Area menggambar                         |
| `xctk:ColorPicker` | Pemilih warna garis dan isi             |
| Tombol             | Memilih bentuk / simpan / hapus         |


currentShape → menyimpan bentuk yang sedang dipilih (Line, Rectangle, Ellipse, Polyline).

tempShape → objek bentuk yang sedang digambar.

startPoint → posisi awal saat klik mouse.

currentPolyline → untuk menggambar garis bersambung (polyline).

a.Canvas_MouseLeftButtonDown
Saat klik kiri dimulai:
Simpan titik awal.
Buat objek bentuk sesuai pilihan (Line, Rectangle, Ellipse, Polyline).
Tambahkan ke drawCanvas.

b. Canvas_MouseMove
Saat mouse digerakkan:
Ubah ukuran bentuk sementara (garis, persegi, atau lingkaran) sesuai posisi kursor.

c. Canvas_MouseLeftButtonUp
Saat mouse dilepas:
Selesai menggambar, hentikan mode drag.

Erase_Click → menghapus objek terakhir dari kanvas.
Clear_Click → menghapus semua gambar di kanvas.

a. SavePng_Click
Menyimpan isi Canvas sebagai gambar PNG.
Menggunakan RenderTargetBitmap untuk menangkap tampilan kanvas.
Hasil disimpan ke file dengan PngBitmapEncoder.

b. SaveXaml_Click
Menyimpan isi Canvas sebagai file XAML (format asli WPF).
Cocok untuk pengeditan ulang nanti.

c. LoadXaml_Click
Membuka file XAML yang disimpan sebelumnya.
Membaca elemen-elemen dan menampilkannya kembali di drawCanvas.

| Bagian                     | Fungsi                             |
| -------------------------- | ---------------------------------- |
| Variabel utama             | Menyimpan status bentuk dan posisi |
| Tombol bentuk              | Pilih jenis gambar                 |
| Canvas_MouseLeftButtonDown | Mulai menggambar                   |
| Canvas_MouseMove           | Mengubah ukuran bentuk             |
| Canvas_MouseLeftButtonUp   | Selesai menggambar                 |
| Erase_Click                | Hapus gambar terakhir              |
| Clear_Click                | Bersihkan semua                    |
| SavePng_Click              | Simpan gambar ke PNG               |
| SaveXaml_Click             | Simpan ke file XAML                |
| LoadXaml_Click             | Buka file XAML untuk edit ulang    |



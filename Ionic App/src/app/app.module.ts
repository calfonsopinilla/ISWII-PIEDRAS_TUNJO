import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { SplashScreen } from '@ionic-native/splash-screen/ngx';
import { StatusBar } from '@ionic-native/status-bar/ngx';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { ComponentsModule } from './components/components.module';

import { HttpClientModule } from '@angular/common/http';
import { IonicStorageModule } from '@ionic/storage';

import { ImagePicker } from '@ionic-native/image-picker/ngx';
import { Camera, CameraOptions } from '@ionic-native/camera/ngx';
import { WebView } from '@ionic-native/ionic-webview/ngx';

import { NgxQRCodeModule } from 'ngx-qrcode2';
import { BarcodeScanner } from '@ionic-native/barcode-scanner/ngx';

@NgModule({
  declarations: [AppComponent],
  entryComponents: [],
  imports: [
    BrowserModule,
    IonicModule.forRoot(),
    AppRoutingModule,
    HttpClientModule,
    ComponentsModule,
    IonicStorageModule.forRoot(),
    NgxQRCodeModule
  ],
  providers: [
    StatusBar,
    SplashScreen,
    { provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
    ImagePicker,
    Camera,        
    WebView,    
    BarcodeScanner
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
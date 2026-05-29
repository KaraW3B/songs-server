import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { ApiService } from '../../shared/services/api.service';
import { HttpClient, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import {
  faHeart as fasHeart,
  faBars as fasBars,
  faSignOut as fasSignOut,
  faClock as fasClock,
  faLocationDot as fasLocationDot,
  faPhone as fasPhone,
  faAt as fasAt,
  faChevronDown as fasChevronDown,
  faHamburger as fasHamburger,
  faSpinner as fasSpinner,
  faDownload as fasDownload,
  faSquarePlus as fasSquarePlus,
  faCircleChevronLeft as fasCircleChevronLeft,
  faCirclePlay as fasCirclePlay
} from '@fortawesome/free-solid-svg-icons';
import { SharedModules } from 'src/shared/shared.modules';
import { DashboardComponent } from '../dashboard/dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
  ],
  bootstrap: [AppComponent],
  imports: [
    SharedModules,
    DashboardComponent,
    AppRoutingModule
  ],
  providers: [ApiService, HttpClient, provideHttpClient(withInterceptorsFromDi())]
})

export class AppModule {
  constructor(library: FaIconLibrary) {
    library.addIcons(fasHeart, fasBars, fasSignOut, fasClock, fasLocationDot, fasPhone, fasAt, fasChevronDown, fasCirclePlay, fasSpinner, fasHamburger, fasDownload, fasSquarePlus, fasCircleChevronLeft);
  }
}

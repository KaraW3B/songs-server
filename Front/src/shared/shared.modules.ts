import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";

@NgModule({
    imports: [
        BrowserModule,
        RouterModule,
        NgbModule,
        FontAwesomeModule
    ],
    exports: [
        BrowserModule,
        RouterModule,
        NgbModule,
        FontAwesomeModule
    ]
})
export class SharedModules { }
import { MessageApiService } from './services/MessageApiService';
import { MessageService } from './services/message.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';

import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { NavigationComponent } from './navigation/navigation.component';
import { InboxesComponent } from './inboxes/inboxes.component';
import { InboxComponent } from './inbox/inbox.component';
import { HomeComponent } from './home/home.component';
import { SettingsComponent } from './settings/settings.component';
import { MessageComponent } from './message/message.component';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NotificationsService } from './services/notifications.service';
import { InboxService } from './services/inbox.service';
import { InboxApiService } from './services/InboxApiService';
import { HttpModule } from '@angular/http';
import { InboxSettingsComponent } from './inbox-settings/inbox-settings.component';
import { MomentModule } from 'angular2-moment';
import { SmtpSettingsComponent } from './settings/smtp-settings/smtp-settings.component';
import { FilterListComponent } from './settings/filter-list/filter-list.component';
import { EditFilterComponent } from './settings/edit-filter/edit-filter.component';
import { SettingsHomeComponent } from './settings/settings-home/settings-home.component';
import { ConfirmService, ConfirmState, ConfirmModalComponent, ConfirmTemplateDirective } from './utils/confirm-modal-and-service';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    InboxesComponent,
    InboxComponent,
    HomeComponent,
    SettingsComponent,
    MessageComponent,
    InboxSettingsComponent,
    SmtpSettingsComponent,
    FilterListComponent,
    EditFilterComponent,
    SettingsHomeComponent,
    ConfirmModalComponent,
    ConfirmTemplateDirective
  ],
  imports: [
    NgbModule.forRoot(),
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpModule,
    BrowserAnimationsModule,
    MomentModule
  ],
  providers: [
    NotificationsService,
    InboxService,
    InboxApiService,
    MessageService,
    MessageApiService,
    ConfirmService,
    ConfirmState
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

import { SettingsHomeComponent } from './settings/settings-home/settings-home.component';
import { InboxSettingsComponent } from './inbox-settings/inbox-settings.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { SettingsComponent } from './settings/settings.component';
import { InboxComponent } from './inbox/inbox.component';
import { MessageComponent } from './message/message.component';
import { EditFilterComponent } from './settings/edit-filter/edit-filter.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'settings',
    component: SettingsComponent,
    children: [
      {
        path: '',
        component: SettingsHomeComponent
      },
      {
        path: 'filter/:id',
        component: EditFilterComponent
      }
    ]
  },
  {
    path: 'inbox/:id',
    component: InboxComponent,
    children: [
      {
        path: 'settings',
        component: InboxSettingsComponent
      },
      {
        path: ':id',
        component: MessageComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

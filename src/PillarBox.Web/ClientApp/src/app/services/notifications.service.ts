import { Observable } from 'rxjs/Observable';
import { MessageSummaryModel } from './../models/MessageSummaryModel.model';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Injectable } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from "@aspnet/signalr";
import { NotificationModel } from '../models/NotificationModel.model';
import { UserInboxModel } from '../models/UserInboxModel.model';
import { InboxService } from './inbox.service';

declare var Notification: any;

@Injectable()
export class NotificationsService {

    private hubConnection: HubConnection;

    private inboxes = new BehaviorSubject<Array<UserInboxModel>>([]);
    inboxRoot = this.inboxes.asObservable();

    private messages = new BehaviorSubject<MessageSummaryModel>(null);
    messages$ = this.messages.asObservable();

    private connectionSubject = new BehaviorSubject<boolean>(null);
    connectionState = this.connectionSubject.asObservable();

    private connected: Boolean;

    constructor() {

        this.hubConnection = new signalR.HubConnectionBuilder().withUrl("/notifications").build();

        this.ConnectToHub();
        this.connectionState.subscribe(s => this.connected = s);
        this.hubConnection.onclose((e) => {
            this.connectionSubject.next(false);
            setTimeout(this.ConnectToHub, 3000);//3s retry
        });

        if (Notification && Notification.permission !== "denied") {
            Notification.requestPermission(function (status: string) {  // status is "granted", if accepted by user
            });
        }
        
        this.hubConnection.on('Notify', (notification) => {
            this.showNotification(notification);
        });

        this.hubConnection.on('InboxUpdate', (i) => this.inboxUpdate(i));

        this.hubConnection.on('InboxMessage', (summary) => {
            this.messages.next(summary);
        });

    }

    private ConnectToHub() {
        this.hubConnection
            .start()
            .then(() => {
                console.log('Connection started!');
                this.connectionSubject.next(true);
            })
            .catch(err => {
                this.connectionSubject.next(false);
                console.log('Error while establishing connection :(');
            });
    }

    subscribeInbox(id: string) {
        if (this.connected) {
            this.hubConnection.send('subscribeInbox', id);
        }
    }

    inboxUpdate(inboxes: UserInboxModel[]) {
        this.inboxes.next(inboxes);
    }

    showNotification(notification: NotificationModel) {

        //console.log(notification);

        var n = new Notification(notification.subject, {
            body: notification.from,
            icon: '/assets/images/notification.png', // optional
            tag: notification.id // this should be message id, stops duplicates when multiple tabs are open
        });
        n.onclick = function (event) {
            console.log(event.target.tag);
            event.target.close();
        };
        setTimeout(n.close.bind(n), 4000);
    }

}


// More info: http://frhagn.github.io/Typewriter/




export class UserInboxModel {
    
    // INBOXNAME
    public inboxName: string = null;
    // INBOXID
    public inboxId: string = null;
    // INBOXPARENTINBOXID
    public inboxParentInboxId: string = null;
    // CHILDREN
    public children: UserInboxModel[] = [];
    // MESSAGECOUNT
    public messageCount: number = 0;
    // COUNTALL
    public countAll: number = 0;
    // STARRED
    public starred: boolean = false;
    // FULLPATH
    public fullPath: string = null;
}

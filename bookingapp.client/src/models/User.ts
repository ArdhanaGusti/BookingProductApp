export interface User {
    id:              number;
    userName:        string;
    fullName:        string;
    email:           string;
    emailVerifiedAt: Date | null;
    password:        string;
}
enum Colour { Red, Green, Blue };

class Student {
    fullname: string;
    constructor(public firstname : string, public middleinitial : string, public lastname) {
        this.fullname = firstname + " " + middleinitial + " " + lastname;
    }
}

interface Person {
    firstname: string;
    lastname: string;
}

function greeter(person: Person) : string {
    return "Hello, " + person.firstname + " " + person.lastname;
}

var user: Student;
var thing: number = 1;

user = new Student("Jane", "m", "User");

document.body.innerHTML = greeter(user);

var c: Colour = Colour.Blue;

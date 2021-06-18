const files = [
{
    id: 1,
    title: 'Speech.pdf',
    status: true,
    feelings: [{"Name":"Positive","Score":34.33}, {"Name":"Negative","Score":61.33}, {"Name":"Neutral","Score":4.33}],
    obscene_language: [],
    url: 'https://soafiles.blob.core.windows.net/files/REST_Tutorial.pdf',
    userDocumentReferences: [
        {name: "José Montoya", qty: 8},
        {name: "Kurt Cobain", qty: 27}
    ],
    owner: "John Doe"
},
{
    id: 2,
    title: 'LoveLetter.txt',
    status: true,
    feelings: [],
    obscene_language: ["fuck","shit","motherfucker","crap"],
    url: 'https://soafiles.blob.core.windows.net/files/REST_Tutorial.pdf',
    userDocumentReferences: [
        {name: "José Montoya", qty: 8},
        {name: "Billie Joe Armstrong", qty: 15},
        {name: "Ash Ketchum", qty: 200},
    ],
    owner: "Jane Doe"
},
{
    id: 3,
    title: 'AnotherLetter.docx',
    status: true,
    feelings: [{"Name":"Positive","Score":50}, {"Name":"Negative","Score":40}, {"Name":"Neutral","Score":10}],
    obscene_language: [],
    url: 'https://soafiles.blob.core.windows.net/files/REST_Tutorial.pdf',
    userDocumentReferences: [
        {name: "Billie Joe Armstrong", qty: 15},
        {name: "Ash Ketchum", qty: 200},
    ],
    owner: "Jane Doe"
},
{
    id: 4,
    title: 'LoveLetter.txt',
    status: false,
    feelings: [],
    obscene_language: ["fuck","shit","motherfucker","crap"],
    url: 'https://soafiles.blob.core.windows.net/files/REST_Tutorial.pdf',
    userDocumentReferences: [
        {name: "José Montoya", qty: 8},
        {name: "Billie Joe Armstrong", qty: 15},
        {name: "Ash Ketchum", qty: 200},
    ],
    owner: "Jane Doe"
},
{
    id: 5,
    title: 'LoveLetter.txt',
    status: true,
    feelings: [],
    obscene_language: [],
    url: 'https://soafiles.blob.core.windows.net/files/REST_Tutorial.pdf',
    userDocumentReferences: [
        {name: "José Montoya", qty: 8},
        {name: "Billie Joe Armstrong", qty: 15},
        {name: "Ash Ketchum", qty: 200},
    ],
    owner: "Jane Doe"
},
{
    id: 6,
    title: 'El quijote.txt',
    status: false,
    feelings: [],
    obscene_language: [],
    url: 'https://soafiles.blob.core.windows.net/files/REST_Tutorial.pdf',
    userDocumentReferences: [
        {name: "José Montoya", qty: 8},
        {name: "Billie Joe Armstrong", qty: 15},
        {name: "Ash Ketchum", qty: 200},
    ],
    owner: "John Doe"
},
{
    id: 7,
    title: 'LoveLetter.txt',
    status: false,
    feelings: [],
    obscene_language: [],
    url: 'https://soafiles.blob.core.windows.net/files/REST_Tutorial.pdf',
    userDocumentReferences: [
        {name: "José Montoya", qty: 8},
        {name: "Billie Joe Armstrong", qty: 15},
        {name: "Ash Ketchum", qty: 200},
    ],
    owner: "Jane Doe"
},
{
    id: 8,
    title: 'LoveLetter.txt',
    status: true,
    feelings: [],
    obscene_language: [],
    url: 'https://soafiles.blob.core.windows.net/files/REST_Tutorial.pdf',
    userDocumentReferences: [
        {name: "José Montoya", qty: 8},
        {name: "Billie Joe Armstrong", qty: 15},
        {name: "Ash Ketchum", qty: 200},
    ],
    owner: "Jane Doe"
},
{
    id: 9,
    title: 'LoveLetter.txt',
    status: true,
    feelings: [],
    obscene_language: [],
    url: 'https://soafiles.blob.core.windows.net/files/REST_Tutorial.pdf',
    userDocumentReferences: [
    ],
    owner: "Jane Doe"
},
{
    id: 10,
    title: 'HolyBlible.pdf',
    status: true,
    feelings: [],
    obscene_language: [],
    url: 'https://soafiles.blob.core.windows.net/files/REST_Tutorial.pdf',
    userDocumentReferences: [
        {name: "Dios", qty: 8888},
    ],
    owner: "Jhon"
}
];
  
export default files;
  
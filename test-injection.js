
const projectPath = '/Users/alexsandrocruz/Documents/dev/AbpZen/demo-zen';

const entities = [
    { name: 'Lead', pluralName: 'Leads' },
    { name: 'LeadContact', pluralName: 'LeadContacts' },
    { name: 'TestEntity', pluralName: 'TestEntities' }
];

const camelCase = (str) => str.charAt(0).toLowerCase() + str.slice(1);

const instructions = entities.flatMap(entity => [
    {
        file: 'LeptonXDemoApp.Web/Menus/LeptonXDemoAppMenus.cs',
        marker: 'ZenCode-Menus-Marker',
        content: `        public const string ${entity.name} = Prefix + ".${entity.name}";`
    },
    {
        file: 'LeptonXDemoApp.Web/Menus/LeptonXDemoAppMenuContributor.cs',
        marker: 'ZenCode-Menu-Marker',
        content: `            context.Menu.AddItem(new ApplicationMenuItem(LeptonXDemoAppMenus.${entity.name}, l["Menu:${entity.pluralName}"], "~/${entity.name}", icon: "fa fa-folder-open").RequirePermissions(LeptonXDemoAppPermissions.${entity.name}.Default));`
    },
    {
        file: 'LeptonXDemoApp.MongoDB/MongoDb/LeptonXDemoAppMongoDbContext.cs',
        marker: 'ZenCode-MongoCollections-Marker',
        content: `        public IMongoCollection<${entity.name}> ${entity.pluralName} => Collection<${entity.name}>();`
    },
    {
        file: 'LeptonXDemoApp.Application.Contracts/Permissions/LeptonXDemoAppPermissions.cs',
        marker: 'ZenCode-Permissions-Marker',
        content: `        public static class ${entity.name}\n        {\n            public const string Default = GroupName + ".${entity.name}";\n            public const string Create = Default + ".Create";\n            public const string Update = Default + ".Update";\n            public const string Delete = Default + ".Delete";\n        }`
    },
    {
        file: 'LeptonXDemoApp.Application.Contracts/Permissions/LeptonXDemoAppPermissionDefinitionProvider.cs',
        marker: 'ZenCode-PermissionDefinition-Marker',
        content: `            var ${camelCase(entity.name)}Permission = myGroup.AddPermission(LeptonXDemoAppPermissions.${entity.name}.Default, L("Permission:${entity.name}"));\n            ${camelCase(entity.name)}Permission.AddChild(LeptonXDemoAppPermissions.${entity.name}.Create, L("Permission:Create"));\n            ${camelCase(entity.name)}Permission.AddChild(LeptonXDemoAppPermissions.${entity.name}.Update, L("Permission:Update"));\n            ${camelCase(entity.name)}Permission.AddChild(LeptonXDemoAppPermissions.${entity.name}.Delete, L("Permission:Delete"));`
    }
]);

fetch('http://localhost:3001/api/inject-code', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
        projectPath,
        instructions
    })
})
    .then(res => res.json())
    .then(data => {
        console.log(JSON.stringify(data, null, 2));
    })
    .catch(err => console.error(err));

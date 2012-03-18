using System.Data.Metadata.Edm;
using System.Data.Objects.DataClasses;
using WWPlatform.Model.CNTV;
//using WWPlatform.Model.Fiction;
using WWPlatform.Model.RefData;
using WWPlatform.Model.ATK;

[assembly: EdmSchemaAttribute()]
[assembly: EdmRelationshipAttribute("WWPlatform.Model", "fk_catalog", "catalog", RelationshipMultiplicity.One, typeof(Catalog), "feature", RelationshipMultiplicity.Many, typeof(Feature))]
[assembly: EdmRelationshipAttribute("WWPlatform.Model", "fk_feature", "feature", RelationshipMultiplicity.One, typeof(Feature), "webcast", RelationshipMultiplicity.Many, typeof(Webcast))]
[assembly: EdmRelationshipAttribute("WWPlatform.Model", "fk_script", "script", RelationshipMultiplicity.ZeroOrOne, typeof(Script), "webcast", RelationshipMultiplicity.One, typeof(Webcast))]
[assembly: EdmRelationshipAttribute("WWPlatform.Model", "fk_lectuer", "lectuer", RelationshipMultiplicity.One, typeof(Lectuer), "feature", RelationshipMultiplicity.Many, typeof(Feature))]
[assembly: EdmRelationshipAttribute("WWPlatform.Model", "fk_dynasty", "dynasty", RelationshipMultiplicity.One, typeof(Dynasty), "feature", RelationshipMultiplicity.Many, typeof(Feature))]
[assembly: EdmRelationshipAttribute("WWPlatform.Model", "fk_offering_feature", "feature", RelationshipMultiplicity.One, typeof(Feature), "offering", RelationshipMultiplicity.Many, typeof(Offering))]
[assembly: EdmRelationshipAttribute("WWPlatform.Model", "fk_offering_subtype", "subtype", RelationshipMultiplicity.One, typeof(Subtype), "offering", RelationshipMultiplicity.Many, typeof(Offering))]
//[assembly: EdmRelationshipAttribute("WWPlatform.Model", "fk_chapter_book", "book", RelationshipMultiplicity.One, typeof(Book), "chapter", RelationshipMultiplicity.Many, typeof(Chapter))]
//[assembly: EdmRelationshipAttribute("WWPlatform.Model", "fk_section_book", "book", RelationshipMultiplicity.One, typeof(Book), "section", RelationshipMultiplicity.Many, typeof(Section))]
//[assembly: EdmRelationshipAttribute("WWPlatform.Model", "fk_chapter_section", "section", RelationshipMultiplicity.One, typeof(Section), "chapter", RelationshipMultiplicity.Many, typeof(Chapter))]
[assembly: EdmRelationshipAttribute("WWPlatform.Model", "fk_class", "atkclass", RelationshipMultiplicity.One, typeof(AtkClass), "digest", RelationshipMultiplicity.Many, typeof(History))]

Feature: Factions

Characters may belong to one or more Factions. A character can join and leave a Faction, and newly created characters
do not belong to any Faction.

Characters belonging to the same Faction are considered Allies. Allies are not able to Damage one another,
but are able to Heal one another.

    Scenario: New characters do not belong to any factions
        When A new character "Naked Pierce Hawthorne" is created
        Then "Naked Pierce Hawthorne" should not belong to any factions

    Scenario: Two characters are considered Allies once they join the same faction
        Given the following characters:
          | Name        |
          | Brutalitops |
          | Lavernica   |
        Then "Brutalitops" and "Lavernica" should not be allies
        When "Lavernica" joins "Treasure Hunters"
        Then "Brutalitops" and "Lavernica" should not be allies
        When "Brutalitops" joins "Treasure Hunters"
        Then "Brutalitops" and "Lavernica" should be allies
        When "Lavernica" leaves "Treasure Hunters"
        Then "Brutalitops" and "Lavernica" should not be allies

    Scenario: Characters can belong to multiple factions, but are only allies with characters they share a faction with
        Given the following characters:
          | Name        |
          | Marrrrr     |
          | Zippadeedoo |
          | Brutalitops |
        When "Marrrrr" joins "Study Group"
        And "Zippadeedoo" joins "Study Group"
        And "Zippadeedoo" joins "Spanish Class"
        And "Brutalitops" joins "Spanish Class"
        Then "Marrrrr" and "Zippadeedoo" should be allies
        And "Zippadeedoo" and "Brutalitops" should be allies
        But "Marrrrr" and "Brutalitops" should not be allies

    Scenario: Allied characters cannot Damage one another
        Given the following characters:
          | Name                    |
          | Hector The Well-Endowed |
          | Lavernica               |
        When "Hector The Well-Endowed" and "Lavernica" are allies
        Then "Hector The Well-Endowed" should not be able to damage "Lavernica"
        And "Lavernica" should not be able to damage "Hector The Well-Endowed"
        
    Scenario: Allied characters can Heal one another
        Given the following characters:
          | Name                 |
          | Lavernica            |
          | Bing Bong The Archer |
        When "Lavernica" and "Bing Bong The Archer" are allies
        Then "Bing Bong The Archer" should be able to heal "Lavernica"
        And "Lavernica" should be able to heal "Bing Bong The Archer"
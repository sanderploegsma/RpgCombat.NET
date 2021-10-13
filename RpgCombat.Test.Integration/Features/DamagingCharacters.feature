Feature: Damaging characters
Characters can Damage other non-allied characters if they are in range.
The maximum range depends on the character class.
Damage taken by a character is subtracted from the character's Health.
If the Health drops to or below zero, that character dies.

The Damage dealt between characters depends on both the attacker's and target's Level.
If the target is 5 or more Levels above the attacker, Damage is reduced by 50%.
If the target is 5 or more Levels below the attacker, Damage is increased by 50%.

    Scenario: Character deals less damage than the other character's health
        Given the following characters:
          | Name                    | Health |
          | Hector The Well-Endowed |        |
          | Lavernica               | 1000   |
        When "Hector The Well-Endowed" attempts to deal 100 damage to "Lavernica"
        Then the health of "Lavernica" should be 900
        And "Lavernica" should be Alive

    Scenario: Character deals more damage than the other character's health
        Given the following characters:
          | Name                    | Health |
          | Hector The Well-Endowed |        |
          | Lavernica               | 100    |
        When "Hector The Well-Endowed" attempts to deal 200 damage to "Lavernica"
        Then "Lavernica" should be Dead
        And the health of "Lavernica" should be 0

    Scenario: Characters cannot deal negative damage
        Given the following characters:
          | Name                    | Health |
          | Hector The Well-Endowed |        |
          | Lavernica               | 800    |
        When "Hector The Well-Endowed" attempts to deal -200 damage to "Lavernica"
        Then the health of "Lavernica" should be 800

    Scenario: Dead characters cannot be damaged
        Given the following characters:
          | Name                    | Health |
          | Hector The Well-Endowed |        |
          | Lavernica               | 0      |
        Then "Hector The Well-Endowed" should not be able to damage "Lavernica"

    Scenario: Characters cannot damage themselves
        Given the following characters:
          | Name                    | Health |
          | Hector The Well-Endowed | 1000   |
        Then "Hector The Well-Endowed" should not be able to damage "Hector The Well-Endowed"

    Scenario: Dead characters cannot damage other characters
        Given the following characters:
          | Name                    | Health |
          | Hector The Well-Endowed | 0      |
          | Lavernica               | 1000   |
        Then "Hector The Well-Endowed" should not be able to damage "Lavernica"

    Scenario: Melee characters have a maximum range of 2
        Given the following characters:
          | Name                    | Class | X  | Y |
          | Hector The Well-Endowed | Melee | 0  | 0 |
          | Brutalitops             |       | 1  | 0 |
          | Duquesne                |       | 1  | 1 |
          | Zippadeedoo             |       | 2  | 1 |
          | Bing Bong The Archer    |       | -2 | 2 |
        Then "Hector The Well-Endowed" should be able to damage "Brutalitops"
        And "Hector The Well-Endowed" should be able to damage "Duquesne"
        But "Hector The Well-Endowed" should not be able to damage "Zippadeedoo"
        And "Hector The Well-Endowed" should not be able to damage "Bing Bong The Archer"

    Scenario: Ranged characters have a maximum range of 20
        Given the following characters:
          | Name                    | Class  | X  | Y   |
          | Bing Bong The Archer    | Ranged | 0  | 0   |
          | Duquesne                |        | -3 | 2   |
          | Brutalitops             |        | 5  | -8  |
          | Lavernica               |        | 15 | -15 |
          | Hector The Well-Endowed |        | 21 | 0   |
        Then "Bing Bong The Archer" should be able to damage "Duquesne"
        And "Bing Bong The Archer" should be able to damage "Brutalitops"
        But "Bing Bong The Archer" should not be able to damage "Lavernica"
        And "Bing Bong The Archer" should not be able to damage "Hector The Well-Endowed"

    Scenario Outline: Characters with a level difference of less than 5 have no damage multiplier
        Given the following characters:
          | Name        | Health | Level           |
          | Marrrrrr    | 1000   | <AttackerLevel> |
          | Zippadeedoo | 1000   | <DefenderLevel> |
        When "Marrrrrr" attempts to deal 100 damage to "Zippadeedoo"
        Then the health of "Zippadeedoo" should be 900

        Examples:
          | AttackerLevel | DefenderLevel |
          | 1             | 1             |
          | 2             | 4             |
          | 5             | 2             |
          | 6             | 6             |

    Scenario Outline: When dealing damage to a character that is 5 or more levels lower, the damage is increased by 50%
        Given the following characters:
          | Name        | Health | Level           |
          | Brutalitops | 1000   | <AttackerLevel> |
          | Lavernica   | 1000   | <DefenderLevel> |
        When "Brutalitops" attempts to deal 100 damage to "Lavernica"
        Then the health of "Lavernica" should be 850

        Examples:
          | AttackerLevel | DefenderLevel |
          | 20            | 1             |
          | 10            | 3             |
          | 8             | 3             |

    Scenario Outline: When dealing damage to a character that is 5 or more levels higher, the damage is decreased by 50%
        Given the following characters:
          | Name                   | Health | Level           |
          | Naked Pierce Hawthorne | 1000   | <AttackerLevel> |
          | Duquesne               | 1000   | <DefenderLevel> |
        When "Naked Pierce Hawthorne" attempts to deal 100 damage to "Duquesne"
        Then the health of "Duquesne" should be 950

        Examples:
          | AttackerLevel | DefenderLevel |
          | 5             | 10            |
          | 1             | 100           |
          | 25            | 42            |
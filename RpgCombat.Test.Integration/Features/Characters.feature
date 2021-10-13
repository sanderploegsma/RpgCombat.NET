Feature: Characters

    Scenario: When creating a new character, it has maximum health, minimum level, and is alive
        When A new character "Marrrrr" is created
        Then the health of "Marrrrr" should be 1000
        And the level of "Marrrrr" should be 1
        And "Marrrrr" should be Alive

    Scenario Outline: When a character's health drops to or below zero, it is considered dead
        Given the following characters:
          | Name        | Health   |
          | Brutalitops | <Health> |
        Then "Brutalitops" should be Dead

        Examples:
          | Health |
          | 0      |
          | -100   |
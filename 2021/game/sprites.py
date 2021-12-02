import pygame

class Background(pygame.sprite.Sprite):
    def __init__(self, image_file, location):
        pygame.sprite.Sprite.__init__(self)  #call Sprite initializer
        self.image = pygame.image.load(image_file)
        self.rect = self.image.get_rect()
        self.rect.left, self.rect.top = location

    def draw(self, surface: pygame.Surface):
        surface.blit(self.image, self.rect)


class DaySprit(pygame.sprite.Sprite):
    def __init__(self, image_file, location, day):
        pygame.sprite.Sprite.__init__(self)  #call Sprite initializer
        self.image = pygame.image.load(image_file)
        self.rect = self.image.get_rect()
        self.rect.left, self.rect.top = location
        self.day_number = day

    def draw(self, surface: pygame.Surface):
        surface.blit(self.image, self.rect)


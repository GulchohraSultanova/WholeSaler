using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.Contacts;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.Contact;
using WholeSalers.Application.GlobalAppException;
using WholeSalers.Domain.Entities;
using WholeSalers.Domain.HelperEntities;

namespace WholeSalers.Persistance.Concretes.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactReadRepository _contactReadRepo;
        private readonly IContactWriteRepository _contactWriteRepo;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public ContactService(
            IContactReadRepository contactReadRepo,
            IContactWriteRepository contactWriteRepo,
            IMailService mailService,
            IMapper mapper)
        {
            _contactReadRepo = contactReadRepo;
            _contactWriteRepo = contactWriteRepo;
            _mailService = mailService;
            _mapper = mapper;
        }

        public async Task<List<ContactDto>> GetAllAsync()
        {
            var contacts = await _contactReadRepo.GetAllAsync(c => !c.IsDeleted);
            return _mapper.Map<List<ContactDto>>(contacts.ToList());
        }

        public async Task<ContactDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış Contact ID!");

            var contact = await _contactReadRepo.GetAsync(c => c.Id == id && !c.IsDeleted);

            if (contact == null)
                throw new GlobalAppException("Contact tapılmadı!");

            return _mapper.Map<ContactDto>(contact);
        }

        public async Task CreateAsync(CreateContactDto dto)
        {
            if (dto == null)
                throw new GlobalAppException("Göndərilən məlumat boşdur!");

            var contact = _mapper.Map<Contact>(dto);

            await _contactWriteRepo.AddAsync(contact);
            await _contactWriteRepo.CommitAsync();

            // ✅ send email notification (optional)
            var mail = new MailRequest
            {
                ToEmail = "admin@wholesalers.com", // change this
                Subject = $"New Contact Message: {dto.Name} {dto.SurName}",
                Body = $@"
        <div style='font-family:Arial,Helvetica,sans-serif;background:#f6f7fb;padding:24px;'>
            <div style='max-width:640px;margin:0 auto;background:#ffffff;border-radius:12px;padding:20px;
                        border:1px solid #e9e9ef;box-shadow:0 6px 18px rgba(0,0,0,0.06);'>
                
                <h2 style='margin:0 0 12px 0;color:#111827;'>Yeni Əlaqə Mesajı</h2>
                <p style='margin:0 0 16px 0;color:#6b7280;font-size:14px;'>
                    Sayt üzərindən yeni mesaj göndərildi.
                </p>

                <div style='border-top:1px solid #f0f0f5;margin:16px 0;'></div>

                <table style='width:100%;border-collapse:collapse;font-size:14px;color:#111827;'>
                    <tr>
                        <td style='padding:10px 0;width:140px;color:#6b7280;'>Ad:</td>
                        <td style='padding:10px 0;font-weight:600;'>{dto.Name}</td>
                    </tr>
                    <tr>
                        <td style='padding:10px 0;color:#6b7280;'>Soyad:</td>
                        <td style='padding:10px 0;font-weight:600;'>{dto.SurName}</td>
                    </tr>
                    <tr>
                        <td style='padding:10px 0;color:#6b7280;'>Email:</td>
                        <td style='padding:10px 0;'>
                            <a href='mailto:{dto.Email}' style='color:#2563eb;text-decoration:none;'>{dto.Email}</a>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding:10px 0;color:#6b7280;'>Telefon:</td>
                        <td style='padding:10px 0;font-weight:600;'>{dto.Phone}</td>
                    </tr>
                </table>

                <div style='border-top:1px solid #f0f0f5;margin:16px 0;'></div>

                <div style='font-size:14px;color:#6b7280;margin-bottom:6px;'>Mesaj:</div>
                <div style='background:#f9fafb;border:1px solid #eef2f7;border-radius:10px;padding:12px;
                            white-space:pre-wrap;color:#111827;line-height:1.5;'>
                    {dto.Message}
                </div>

                <div style='margin-top:18px;font-size:12px;color:#9ca3af;'>
                    Bu email avtomatik göndərilib.
                </div>
            </div>
        </div>"
            };


            await _mailService.SendEmailAsync(mail);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış Contact ID!");

            var contact = await _contactReadRepo.GetAsync(c => c.Id == id && !c.IsDeleted)
                          ?? throw new GlobalAppException("Contact tapılmadı!");

            await _contactWriteRepo.SoftDeleteAsync(contact);
            await _contactWriteRepo.CommitAsync();
        }
    }
}

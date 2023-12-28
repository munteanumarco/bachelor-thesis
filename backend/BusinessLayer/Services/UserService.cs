using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using DataAccessLayer.Interfaces;

namespace BusinessLayer.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }   
    
    public async Task<IEnumerable<GetUserDto>> GetUsersAsync()
    {
        return _mapper.Map<IEnumerable<GetUserDto>>(await _userRepository.GetUsersAsync());
    }
}
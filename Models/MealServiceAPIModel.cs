using System.ComponentModel.DataAnnotations;

public class MealServiceAPIModel
{
    [Required]
    public string KEY { get; set; } //인증키
    [Required]
    public string Type { get; set; } //호출문서 (JSON 고정 권장)
    [Required]
    public int pIndex { get; set; } //페이지 위치 (1 고정)
    [Required]
    public int pSize { get; set; } //페이지당 신청 숫자
    public string ATPT_OFCDC_SC_CODE { get; set; } //시도교육청코드
    public string SD_SCHUL_CODE { get; set; } //표준학교 코드
    public string MMEAL_SC_CODE { get; set; } //식사코드
    public string MLSV_YMD { get; set; } //급식일자
    //자세한 정보는 https://open.neis.go.kr/portal/data/service/selectServicePage.do?page=1&rows=10&sortColumn=&sortDirection=&infId=OPEN17320190722180924242823&infSeq=2 참조
}
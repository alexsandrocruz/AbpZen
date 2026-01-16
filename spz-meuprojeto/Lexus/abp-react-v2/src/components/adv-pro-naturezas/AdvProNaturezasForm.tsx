import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateAdvProNaturezas, useUpdateAdvProNaturezas } from "@/lib/abp/hooks/useAdvProNaturezases";
import { toast } from "sonner";

const formSchema = z.object({
  
    idNatureza: z.any(),
  
    titulo: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    mostraHistoricoNumeros: z.any(),
  
    recebeAcordo: z.any(),
  
    recebeRPV: z.any(),
  
    recebePrecatorio: z.any(),
  
    recebeAlvara: z.any(),
  
    idArea: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvProNaturezasFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvProNaturezasForm({
  isOpen,
  onClose,
  initialValues,
}: AdvProNaturezasFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateAdvProNaturezas();
const updateMutation = useUpdateAdvProNaturezas();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advProNaturezas updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advProNaturezas created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advpronaturezas:", error);
    toast.error(error.message || "Failed to save advpronaturezas");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advProNaturezas": "Create advProNaturezas" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advpronaturezas." : "Fill in the details to create a new advpronaturezas." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idNatureza" > idNatureza</Label>

<Input id="idNatureza" type = "number" step = "any" {...register("idNatureza") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="titulo" > titulo</Label>

<Input id="titulo" {...register("titulo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ativo" > ativo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="ativo"
checked = { watch("ativo") }
onCheckedChange = {(checked) => setValue("ativo", !!checked)}
              />
  < label htmlFor = "ativo" className = "text-sm font-normal" >
    { watch("ativo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="mostraHistoricoNumeros" > mostraHistoricoNumeros</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="mostraHistoricoNumeros"
checked = { watch("mostraHistoricoNumeros") }
onCheckedChange = {(checked) => setValue("mostraHistoricoNumeros", !!checked)}
              />
  < label htmlFor = "mostraHistoricoNumeros" className = "text-sm font-normal" >
    { watch("mostraHistoricoNumeros") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recebeAcordo" > recebeAcordo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recebeAcordo"
checked = { watch("recebeAcordo") }
onCheckedChange = {(checked) => setValue("recebeAcordo", !!checked)}
              />
  < label htmlFor = "recebeAcordo" className = "text-sm font-normal" >
    { watch("recebeAcordo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recebeRPV" > recebeRPV</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recebeRPV"
checked = { watch("recebeRPV") }
onCheckedChange = {(checked) => setValue("recebeRPV", !!checked)}
              />
  < label htmlFor = "recebeRPV" className = "text-sm font-normal" >
    { watch("recebeRPV") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recebePrecatorio" > recebePrecatorio</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recebePrecatorio"
checked = { watch("recebePrecatorio") }
onCheckedChange = {(checked) => setValue("recebePrecatorio", !!checked)}
              />
  < label htmlFor = "recebePrecatorio" className = "text-sm font-normal" >
    { watch("recebePrecatorio") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recebeAlvara" > recebeAlvara</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recebeAlvara"
checked = { watch("recebeAlvara") }
onCheckedChange = {(checked) => setValue("recebeAlvara", !!checked)}
              />
  < label htmlFor = "recebeAlvara" className = "text-sm font-normal" >
    { watch("recebeAlvara") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="idArea" > idArea</Label>

<Input id="idArea" type = "number" step = "any" {...register("idArea") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}

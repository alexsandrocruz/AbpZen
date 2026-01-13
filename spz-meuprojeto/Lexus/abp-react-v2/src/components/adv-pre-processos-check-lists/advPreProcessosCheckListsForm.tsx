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
  import { useCreateAdvPreProcessosCheckLists, useUpdateAdvPreProcessosCheckLists } from "@/lib/abp/hooks/useAdvPreProcessosCheckListses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idPreCheckList: z.any(),
  
    idProcesso: z.any(),
  
    idGrupo: z.any(),
  
    idCheckList: z.any(),
  
    tsInclusao: z.any(),
  
    grupo: z.any(),
  
    item: z.any(),
  
    concluido: z.any(),
  
    tsConclusao: z.any(),
  
    idResponsavel: z.any(),
  
    ordem: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvPreProcessosCheckListsFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvPreProcessosCheckListsForm({
  isOpen,
  onClose,
  initialValues,
}: AdvPreProcessosCheckListsFormProps) {
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

  const createMutation = useCreateAdvPreProcessosCheckLists();
const updateMutation = useUpdateAdvPreProcessosCheckLists();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advPreProcessosCheckLists updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advPreProcessosCheckLists created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advpreprocessoschecklists:", error);
    toast.error(error.message || "Failed to save advpreprocessoschecklists");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advPreProcessosCheckLists": "Create advPreProcessosCheckLists" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advpreprocessoschecklists." : "Fill in the details to create a new advpreprocessoschecklists." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idPreCheckList" > idPreCheckList</Label>

<Input id="idPreCheckList" type = "number" step = "any" {...register("idPreCheckList") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso</Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idGrupo" > idGrupo</Label>

<Input id="idGrupo" type = "number" step = "any" {...register("idGrupo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCheckList" > idCheckList</Label>

<Input id="idCheckList" type = "number" step = "any" {...register("idCheckList") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="grupo" > grupo</Label>

<Input id="grupo" {...register("grupo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="item" > item</Label>

<Input id="item" {...register("item") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="concluido" > concluido</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="concluido"
checked = { watch("concluido") }
onCheckedChange = {(checked) => setValue("concluido", !!checked)}
              />
  < label htmlFor = "concluido" className = "text-sm font-normal" >
    { watch("concluido") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsConclusao" > tsConclusao</Label>

<Input id="tsConclusao" {...register("tsConclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idResponsavel" > idResponsavel</Label>

<Input id="idResponsavel" type = "number" step = "any" {...register("idResponsavel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ordem" > ordem</Label>

<Input id="ordem" type = "number" step = "any" {...register("ordem") } />

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

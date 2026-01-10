import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLawyerSpecializationsInput {
  filter?: string;
  skipCount?: number;
  maxResultCount?: number;
}

export function useLawyerSpecializations(input: GetLawyerSpecializationsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10 } = input;

  return useQuery({
    queryKey: ["lawyerSpecializations", filter, skipCount, maxResultCount],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lawyer-specialization", {
        params: {
          filter,
          skipCount,
          maxResultCount,
        },
      });
      return response.data;
    },
  });
}

export function useLawyerSpecialization(id: string) {
  return useQuery({
    queryKey: ["lawyerSpecialization", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lawyer-specialization/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLawyerSpecialization() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lawyer-specialization", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecializations"] });
    },
  });
}

export function useUpdateLawyerSpecialization() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lawyer-specialization/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecializations"] });
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecialization", data.id] });
    },
  });
}

export function useDeleteLawyerSpecialization() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lawyer-specialization/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecializations"] });
    },
  });
}

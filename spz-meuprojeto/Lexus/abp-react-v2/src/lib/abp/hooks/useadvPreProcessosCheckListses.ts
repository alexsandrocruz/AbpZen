import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvPreProcessosCheckListsesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvPreProcessosCheckListses(input: GetAdvPreProcessosCheckListsesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advPreProcessosCheckListses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-processos-check-lists", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllAdvPreProcessosCheckListses() {
  return useQuery({
    queryKey: ["advPreProcessosCheckListses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-processos-check-lists", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvPreProcessosCheckLists(id: string) {
  return useQuery({
    queryKey: ["advPreProcessosCheckLists", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pre-processos-check-lists/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvPreProcessosCheckLists() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pre-processos-check-lists", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreProcessosCheckListses"] });
    },
  });
}

export function useUpdateAdvPreProcessosCheckLists() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pre-processos-check-lists/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advPreProcessosCheckListses"] });
      queryClient.invalidateQueries({ queryKey: ["advPreProcessosCheckLists", data.id] });
    },
  });
}

export function useDeleteAdvPreProcessosCheckLists() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pre-processos-check-lists/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreProcessosCheckListses"] });
    },
  });
}


